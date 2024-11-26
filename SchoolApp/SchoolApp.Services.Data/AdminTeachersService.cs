using Microsoft.EntityFrameworkCore;
using SchoolApp.Data.Models;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.Admin.Teachers;

namespace SchoolApp.Services.Data
{
    public class AdminTeachersService : IAdminTeachersService
    {
        private readonly IRepository _repository;
        private readonly IAzureBlobService _azureBlobService;


        public AdminTeachersService(IRepository repository, IAzureBlobService azureBlobService)
        {
            _repository = repository;
            _azureBlobService = azureBlobService;
        }

        public async Task<IEnumerable<TeacherViewModel>> GetAllTeachersAsync()
        {
            IEnumerable<TeacherViewModel> model = await _repository
                .GetAllAttached<Teacher>()
                .OrderBy(t => t.FirstName)
                .ThenBy(t => t.LastName)
                .Select(t => new TeacherViewModel()
                {
                    GuidId = t.GuidId.ToString(),
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    ImageUrl = t.ImageUrl,
                    JobTitle = t.JobTitle
                })
                .ToArrayAsync();

            return model;
        }

        public async Task<bool> DeleteTeacherAsync(string id)
        {
            bool isIdValid = Guid.TryParse(id, out Guid guidId);

            if (!isIdValid)
            {
                return false;
            }

            Teacher? teacher = await _repository
                        .GetAllAttached<Teacher>()
                        .Include(t => t.SubjectTeachers)
                        .FirstOrDefaultAsync(t => t.GuidId == guidId);


            if (teacher == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(teacher.ImageUrl))
            {
                bool isOldImageDeleted = await _azureBlobService.DeleteTeacherImageAsync(teacher.ImageUrl);
                if (!isOldImageDeleted)
                {
                    return false;
                }
            }

            if (teacher.SubjectTeachers.Any())
            {
                await _repository.DeleteRangeAsync(teacher.SubjectTeachers);
            }

            bool result = await _repository.DeleteByGuidIdAsync<Teacher>(guidId);
            return result;
        }

        public async Task<IEnumerable<SubjectsViewModel>> GetAvailableSubjectsAsync()
        {
            return await _repository.GetAllAttached<Subject>()
                .Select(s => new SubjectsViewModel
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToListAsync();
        }

        public async Task<(bool isSuccessful, string? errorMessage)> AddTeacherAsync(AddTeacherFormModel model)
        {
            // Validate image
            if (model.Image.Length > 2 * 1024 * 1024)
            {
                return (false, "Файлът трябва да е по-малък от 2MB");
            }

            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            string extension = Path.GetExtension(model.Image.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                return (false, "Позволените формати са само JPG, JPEG и PNG");
            }

            // Upload image to Azure and get URL
            var result = await _azureBlobService.UploadTeacherImageAsync(model.Image, model.FirstName, model.LastName);

            if (!string.IsNullOrEmpty(result.errorMessage) || result.imageUrl == null)
            {
                return (false, result.errorMessage);
            }

            var teacher = new Teacher
            {
                GuidId = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                JobTitle = model.JobTitle,
                ImageUrl = result.imageUrl,
                ApplicationUserId = null
            };

            await _repository.AddAsync(teacher);

            foreach (var subjectId in model.SelectedSubjects)
            {
                var subjectTeacher = new SubjectTeacher
                {
                    TeacherId = teacher.GuidId,
                    SubjectId = subjectId
                };

                await _repository.AddAsync(subjectTeacher);
            }

            return (true, null);
        }

        public async Task<EditTeacherFormModel?> GetTeacherForEditAsync(string id)
        {
            bool isIdValid = Guid.TryParse(id, out Guid guidId);
            if (!isIdValid)
            {
                return null;
            }

            var teacher = await _repository
                .GetAllAttached<Teacher>()
                .Include(t => t.SubjectTeachers)
                .FirstOrDefaultAsync(t => t.GuidId == guidId);

            if (teacher == null)
            {
                return null;
            }

            var teacherSubjectIds = teacher.SubjectTeachers.Select(st => st.SubjectId).ToList();

            var availableSubjects = await _repository
                .GetAllAttached<Subject>()
                .Select(s => new SubjectsViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    IsSelected = teacherSubjectIds.Contains(s.Id)
                })
                .ToListAsync();

            string? currentFileName = null;
            if (!string.IsNullOrEmpty(teacher.ImageUrl))
            {
                // Extract filename from URL
                currentFileName = Path.GetFileName(new Uri(teacher.ImageUrl).LocalPath);
            }

            return new EditTeacherFormModel
            {
                Id = teacher.GuidId.ToString(),
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                JobTitle = teacher.JobTitle,
                CurrentImageUrl = teacher.ImageUrl,
                CurrentImageFileName = currentFileName,
                SelectedSubjects = teacherSubjectIds,
                AvailableSubjects = availableSubjects
            };
        }

        public async Task<(bool isSuccessful, string? errorMessage)> EditTeacherAsync(EditTeacherFormModel model)
        {
            bool isIdValid = Guid.TryParse(model.Id, out Guid guidId);
            if (!isIdValid)
            {
                return (false, "Невалидно ID!");
            }

            var teacher = await _repository
                .GetAllAttached<Teacher>()
                .Include(t => t.SubjectTeachers)
                .FirstOrDefaultAsync(t => t.GuidId == guidId);

            if (teacher == null)
            {
                return (false, "Учителят не беше намерен!");
            }

            if (model.Image != null)
            {
                // Validate image
                if (model.Image.Length > 2 * 1024 * 1024)
                {
                    return (false, "Файлът трябва да е по-малък от 2MB");
                }

                string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
                string extension = Path.GetExtension(model.Image.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    return (false, "Позволените формати са само JPG, JPEG и PNG");
                }

                // Delete old image if exists
                if (string.IsNullOrEmpty(teacher.ImageUrl))
                {
                    bool isOldImageDeleted = await _azureBlobService.DeleteTeacherImageAsync(teacher.ImageUrl);
                    if (!isOldImageDeleted)
                    {
                        return (false, "Възникна грешка при изтриването на старата снимка!");
                    }
                }

                // Upload new image
                var result = await _azureBlobService.UploadTeacherImageAsync(model.Image, model.FirstName, model.LastName);
                if (!string.IsNullOrEmpty(result.errorMessage) || result.imageUrl == null)
                {
                    return (false, result.errorMessage);
                }

                teacher.ImageUrl = result.imageUrl;
            }
            else if (string.IsNullOrEmpty(teacher.ImageUrl))
            {
                return (false, "Моля добавете снимка!");
            }

            // Update basic info
            teacher.FirstName = model.FirstName;
            teacher.LastName = model.LastName;
            teacher.JobTitle = model.JobTitle;

            // Update teacher
            bool isTeacherUpdated = await _repository.UpdateAsync(teacher);
            if (!isTeacherUpdated)
            {
                return (false, "Възникна грешка при обновяването на учителя!");
            }

            // Взимаме текущите SubjectId-та
            var currentSubjectIds = teacher.SubjectTeachers.Select(st => st.SubjectId).ToList();

            // Сравняваме дали има разлика между текущите и новите
            bool hasSubjectsChanged = !currentSubjectIds.OrderBy(id => id)
                .SequenceEqual(model.SelectedSubjects.OrderBy(id => id));

            if (hasSubjectsChanged)
            {
                // Delete old subject relations
                await _repository.DeleteRangeAsync(teacher.SubjectTeachers);

                // Add new subject relations
                foreach (var subjectId in model.SelectedSubjects)
                {
                    var subjectTeacher = new SubjectTeacher
                    {
                        TeacherId = teacher.GuidId,
                        SubjectId = subjectId
                    };

                    await _repository.AddAsync(subjectTeacher);
                }
            }

            return (true, null);
        }

        
    }
}