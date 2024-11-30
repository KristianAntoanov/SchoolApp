using Microsoft.EntityFrameworkCore;
using SchoolApp.Data.Models;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels;
using SchoolApp.Web.ViewModels.Admin.Students;

namespace SchoolApp.Services.Data
{
    public class AdminStudentsService : IAdminStudentsService
	{
        private readonly IRepository _repository;

        public AdminStudentsService(IRepository repository)
		{
            _repository = repository;
		}

        public async Task<PaginatedList<StudentsViewModel>> GetAllStudentsAsync(int pageNumber, int pageSize, string searchTerm = null)
        {
            var query = _repository.GetAllAttached<Student>();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string normalizedSearchTerm = searchTerm.ToLower();
                query = query.Where(s =>
                    s.FirstName.ToLower().Contains(normalizedSearchTerm) ||
                    (s.FirstName + " " + s.MiddleName).ToLower().Contains(normalizedSearchTerm) ||
                    (s.FirstName + " " + s.LastName).ToLower().Contains(normalizedSearchTerm) ||
                    (s.FirstName + " " + s.MiddleName + " " + s.LastName).ToLower().Contains(normalizedSearchTerm));
            }

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var students = await query
                .OrderBy(s => s.FirstName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new StudentsViewModel()
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    MiddleName = s.MiddleName,
                    LastName = s.LastName,
                    Grades = s.Grades.Select(g => new GradeViewModel()
                    {
                        GradeValue = g.GradeValue,
                        GradeDate = g.AddedOn,
                        TeacherName = $"{g.Teacher.FirstName} {g.Teacher.LastName}"
                    })
                })
                .ToListAsync();

            return new PaginatedList<StudentsViewModel>
            {
                Items = students,
                PageNumber = pageNumber,
                TotalPages = totalPages,
                TotalItems = totalItems
            };
        }

        public async Task<bool> DeleteStudent(int id)
        {
            Student? student = await _repository
                .FirstOrDefaultAsync<Student>(s => s.Id == id);

            if (student == null)
            {
                return false;
            }

            bool result = await _repository.DeleteAsync<Student>(id);

            return result;
        }

        public async Task<EditStudentFormModel?> GetStudentForEditAsync(int id)
        {
            var student = await _repository.GetAllAttached<Student>()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return null;
            }

            var model = new EditStudentFormModel
            {
                Id = student.Id,
                FirstName = student.FirstName,
                MiddleName = student.MiddleName,
                LastName = student.LastName,
                ClassId = student.ClassId,
                AvailableClasses = await GetAvailableClassesAsync()
            };

            return model;
        }

        public async Task<IList<ClassesViewModel>> GetAvailableClassesAsync()
        {
            return await _repository.GetAllAttached<Class>()
                .Select(c => new ClassesViewModel
                {
                    Id = c.Id,
                    Name = $"{c.GradeLevel} {c.Section.Name}"
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateStudentAsync(EditStudentFormModel model)
        {
            Student? student = await _repository
                .FirstOrDefaultAsync<Student>(s => s.Id == model.Id);

            if (student == null)
            {
                return false;
            }

            student.Id = model.Id;
            student.FirstName = model.FirstName;
            student.LastName = model.LastName;
            student.ClassId = model.ClassId;

            return await _repository.UpdateAsync(student);
        }

        public async Task<StudentGradesManagementViewModel?> GetStudentGradesAsync(int studentId)
        {
            var student = await _repository.GetAllAttached<Student>()
                .Include(s => s.SubjectStudents)
                    .ThenInclude(ss => ss.Subject)
                .Include(s => s.Grades)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                return null;
            }

            var subjectGrades = student.SubjectStudents
                .Select(ss => new SubjectGradesViewModel
                {
                    SubjectId = ss.SubjectId,
                    SubjectName = ss.Subject.Name,
                    Grades = student.Grades
                        .Where(g => g.SubjectId == ss.SubjectId)
                        .Select(g => new GradeManagementViewModel
                        {
                            Id = g.Id,
                            GradeValue = g.GradeValue,
                            GradeDate = g.AddedOn
                        })
                        .ToList()
                })
                .ToList();

            return new StudentGradesManagementViewModel
            {
                StudentId = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                SubjectGrades = subjectGrades
            };
        }

        public async Task<bool> DeleteGradeAsync(int gradeId)
        {
            var grade = await _repository
                    .FirstOrDefaultAsync<Grade>(g => g.Id == gradeId);

            if (grade == null)
            {
                return false;
            }

            return await _repository.DeleteAsync<Grade>(gradeId);
        }

        public async Task<bool> AddStudentAsync(AddStudentFormModel model, string userId)
        {
            if (model == null)
            {
                return false;
            }

            var student = new Student
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                ClassId = model.ClassId,
                ApplicationUserId = Guid.Parse(userId)
            };

            await _repository.AddAsync(student);
            return true;
        }
    }
}

