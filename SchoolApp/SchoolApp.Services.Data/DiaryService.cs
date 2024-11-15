using Microsoft.EntityFrameworkCore;

using SchoolApp.Data.Models;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels;
using SchoolApp.Web.ViewModels.Diary.AddForms;
using SchoolApp.Web.ViewModels.Diary.Remarks;
using static SchoolApp.Common.EntityValidationConstants.Absence;

namespace SchoolApp.Services.Data
{
    public class DiaryService : IDiaryService
    {
        private readonly IRepository _repository;

        public DiaryService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DiaryIndexViewModel>> IndexGetAllClasses()
        {
            IEnumerable<DiaryIndexViewModel> diaries = await _repository
                .GetAllAttached<Class>()
                .OrderBy(c => c.GradeLevel)
                .ThenBy(c => c.SectionId)
                .Select(c => new DiaryIndexViewModel()
                {
                    ClassId = c.Id,
                    ClassName = $"{c.GradeLevel} {c.Section.Name}"
                })
                .ToArrayAsync();

            return diaries;
        }

        public async Task<IEnumerable<StudentGradesViewModel>> GetGradeContent(int classId, int subjectId)
        {
            IEnumerable<StudentGradesViewModel> students = await _repository
                .GetAllAttached<Student>()
                .Where(s => s.ClassId == classId)
                .Select(s => new StudentGradesViewModel()
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Grades = s.Grades
                                .Where(g => g.SubjectId == subjectId)
                                .Select(g => new GradeViewModel()
                                {
                                    GradeValue = g.GradeValue,
                                    GradeDate = g.AddedOn,
                                    TeacherName = $"{g.Teacher.FirstName} {g.Teacher.LastName}"
                                })
                                .ToArray()
                })
                .ToArrayAsync();

            return students;
        }

        public async Task<IEnumerable<SubjectViewModel>> GetClassContent(int classId)
        {
            IEnumerable<SubjectViewModel> subjects = await _repository
                    .GetAllAttached<Subject>()
                    .Select(s => new SubjectViewModel()
                    {
                        Id = s.Id,
                        Name = s.Name
                    })
                    .ToArrayAsync();

            return subjects;
        }

        public async Task<IEnumerable<StudentRemarksViewModel>> GetRemarksContent(int classId)
        {
            IEnumerable<StudentRemarksViewModel> studentRemarks = await _repository
                    .GetAllAttached<Student>()
                    .Where(sr => sr.ClassId == classId)
                    .Select(s => new StudentRemarksViewModel()
                    {
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Remarks = s.Remarks
                                .Select(r => new RemarksViewModel()
                                {
                                    Id = r.Id,
                                    SubjectName = r.Subject.Name,
                                    TeacherName = $"{r.Teacher.FirstName} {r.Teacher.LastName}",
                                    RemarkText = r.RemarkText,
                                    AddedOn = r.AddedOn.ToString(AddedOnDateFormat)
                                })
                    })
                    .ToArrayAsync();

            return studentRemarks;
        }

        public async Task<IEnumerable<StudentAbsencesViewModel>> GetAbsencesContent(int classId)
        {
            IEnumerable<StudentAbsencesViewModel> studentAbsences = await _repository
                    .GetAllAttached<Student>()
                    .Where(sa => sa.ClassId == classId)
                    .Select(s => new StudentAbsencesViewModel()
                    {
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Absences = s.Absences
                                .Select(a => new AbsencesViewModel()
                                {
                                    Id = a.Id,
                                    SubjectName = a.Subject.Name,
                                    AddedOn = a.AddedOn.ToString(AddedOnDateFormat),
                                    IsExcused = a.IsExcused
                                })
                    })
                    .ToArrayAsync();

            return studentAbsences;
        }

        public async Task<T> GetClassStudentForGrades<T>(int classId, int subjectId)
            where T : StudentBaseViewModel, new()
        {
            T model = new()
            {
                AddedOn = DateTime.Now,
                SubjectId = subjectId,
                Subjects = await _repository
                    .GetAllAttached<Subject>()
                    .Select(s => new SubjectViewModel()
                    {
                        Id = s.Id,
                        Name = s.Name
                    })
                    .ToArrayAsync()
            };

            var students = await _repository
                .GetAllAttached<Student>()
                .Where(s => s.ClassId == classId)
                .ToListAsync();

            if (model is GradeFormModel gradeModel)
            {
                gradeModel.Students = students
                    .Select(s => new StudentGradeFormModel()
                    {
                        Id = s.Id,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Grade = 0
                    })
                    .ToList();
            }
            else if (model is AbsenceFormModel absenceModel)
            {
                absenceModel.Students = students
                    .Select(s => new StudentAbcenseFormModel()
                    {
                        Id = s.Id,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        IsChecked = false
                    })
                    .ToList();
            }
            else if (model is RemarkFormModel remarkModel)
            {
                remarkModel.Students = students
                    .Select(s => new StudentRemarkFormModel()
                    {
                        Id = s.Id,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                    })
                    .ToList();
            }

            return model;
        }

        public async Task<bool> AddGrades(string userId, GradeFormModel model)
        {
            Teacher? teacher = await _repository.FirstOrDefaultAsync<Teacher>(t => t.ApplicationUserId == Guid.Parse(userId));

            List<Grade> grades = new List<Grade>();
            foreach (var student in model.Students.Where(s => s.Grade != 0))
            {
                Grade grade = new Grade()
                {
                    AddedOn = model.AddedOn,
                    GradeValue = student.Grade,
                    StudentId = student.Id,
                    SubjectId = model.SubjectId,
                    TeacherId = teacher!.GuidId,
                };
                grades.Add(grade);
            }

            if (grades.Count == 0)
            {
                return false;
            }

            await _repository.AddRangeAsync(grades);

            return true;
        }

        public async Task<bool> AddAbsence(AbsenceFormModel model)
        {
            List<Absence> absences = new List<Absence>();

            foreach (var student in model.Students.Where(s => s.IsChecked))
            {
                Absence absence = new Absence()
                {
                    AddedOn = model.AddedOn,
                    StudentId = student.Id,
                    SubjectId = model.SubjectId,
                };
                absences.Add(absence);
            }

            if (absences.Count == 0)
            {
                return false;
            }

            await _repository.AddRangeAsync(absences);

            return true;
        }

        public async Task<bool> AddRemark(string userId, RemarkFormModel model)
        {
            Teacher? teacher = await _repository.FirstOrDefaultAsync<Teacher>(t => t.ApplicationUserId == Guid.Parse(userId));

            Remark remark = new Remark()
            {
                AddedOn = model.AddedOn,
                RemarkText = model.RemarkText,
                StudentId = model.StudentId,
                SubjectId = model.SubjectId,
                TeacherId = teacher!.GuidId
            };

            await _repository.AddAsync(remark);

            return true;
        }

        public async Task<bool> ExcuseAbsence(int id)
        {
            Absence? absence = await _repository
                .FirstOrDefaultAsync<Absence>(a => a.Id == id);

            if (absence == null)
            {
                return false;
            }

            absence.IsExcused = true;

            await _repository.UpdateAsync(absence);

            return true;
        }

        public async Task<bool> DeleteAbsence(int id)
        {
            bool isDeleted = await _repository
                .DeleteAsync<Absence>(id);

            if (!isDeleted)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteRemark(int id)
        {
            bool isDeleted = await _repository
                .DeleteAsync<Remark>(id);

            if (!isDeleted)
            {
                return false;
            }

            return true;
        }

        public async Task<EditRemarkViewModel?> GetRemarkById(int id)
        {
            Remark? remarkToEdit = await _repository
                .GetByIdAsync<Remark>(id);

            if (remarkToEdit == null)
            {
                return null;
            }

            EditRemarkViewModel model = new EditRemarkViewModel()
            {
                Id = remarkToEdit.Id,
                AddedOn = remarkToEdit.AddedOn,
                SubjectId = remarkToEdit.SubjectId,
                StudentId = remarkToEdit.StudentId,
                RemarkText = remarkToEdit.RemarkText
            };

            return model;
        }

        public async Task<bool> EditRemark(EditRemarkViewModel model)
        {
            Remark? remarkToEdit = await _repository
                .GetByIdAsync<Remark>(model.Id);

            if (remarkToEdit == null)
            {
                return false;
            }

            remarkToEdit.AddedOn = model.AddedOn;
            remarkToEdit.RemarkText = model.RemarkText;

            await _repository.UpdateAsync(remarkToEdit);

            return true;
        }
    }
}