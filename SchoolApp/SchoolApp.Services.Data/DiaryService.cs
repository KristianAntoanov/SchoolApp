using Microsoft.EntityFrameworkCore;

using SchoolApp.Data.Models;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels;
using SchoolApp.Web.ViewModels.Diary.New;

namespace SchoolApp.Services.Data
{
    public class DiaryService : IDiaryService
    {
        private readonly IRepository<Class, int> _classRepository;
        private readonly IRepository<Student, int> _studentRepository;
        private readonly IRepository<Subject, int> _subjectRepository;
        private readonly IRepository<Grade, int> _gradeRepository;
        private readonly IRepository<Teacher, Guid> _teacherRepository;
        private readonly IRepository<Absence, int> _absenceRepository;

        public DiaryService(IRepository<Class, int> classRepository,
                            IRepository<Student, int> studentRepository,
                            IRepository<Subject, int> subjectRepository,
                            IRepository<Grade, int> gradeRepository,
                            IRepository<Teacher, Guid> teacherRepository,
                            IRepository<Absence, int> absenceRepository)
        {
            _classRepository = classRepository;
            _studentRepository = studentRepository;
            _subjectRepository = subjectRepository;
            _gradeRepository = gradeRepository;
            _teacherRepository = teacherRepository;
            _absenceRepository = absenceRepository;
        }

        public async Task<IEnumerable<DiaryIndexViewModel>> IndexGetAllClasses()
        {
            IEnumerable<DiaryIndexViewModel> diaries = await _classRepository
                .GetAllAttached()
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
            IEnumerable<StudentGradesViewModel> students = await _studentRepository
                .GetAllAttached()
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
                                    GradeDate = g.AddedOn
                                })
                                .ToArray()
                })
                .ToArrayAsync();

            return students;
        }

        public async Task<IEnumerable<SubjectsViewModel>> GetClassContent(int classId)
        {
            IEnumerable<SubjectsViewModel> subjects = await _subjectRepository
                    .GetAllAttached()
                    .Select(s => new SubjectsViewModel()
                    {
                        Id = s.Id,
                        SubjectName = s.Name
                    })
                    .ToArrayAsync();

            return subjects;
        }

        public async Task<IEnumerable<StudentRemarksViewModel>> GetRemarksContent(int classId)
        {
            IEnumerable<StudentRemarksViewModel> studentRemarks = await _studentRepository
                    .GetAllAttached()
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
                                    AddedOn = r.AddedOn.ToString()
                                })
                    })
                    .ToArrayAsync();

            return studentRemarks;
        }

        public async Task<IEnumerable<StudentAbsencesViewModel>> GetAbsencesContent(int classId)
        {
            IEnumerable<StudentAbsencesViewModel> studentAbsences = await _studentRepository
                    .GetAllAttached()
                    .Where(sa => sa.ClassId == classId)
                    .Select(s => new StudentAbsencesViewModel()
                    {
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Absences = s.Absences
                                .Select(a => new AbsencesViewModel()
                                {
                                    SubjectName = a.Subject.Name,
                                    AddedOn = a.AddedOn.ToString(),
                                    IsExcused = a.IsExcused
                                })
                    })
                    .ToArrayAsync();

            return studentAbsences;
        }

        public async Task<StudentRecordUpdateModel> GetClassStudentForGrades(int classId, int subjectId)
        {
            IList<StudentModel> students = await _studentRepository
                .GetAllAttached()
                .Where(s => s.ClassId == classId)
                .Select(s => new StudentModel()
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName
                })
                .ToListAsync();

            IEnumerable<SubjectModel> subjects = await _subjectRepository
                .GetAllAttached()
                .Select(s => new SubjectModel()
                {
                    Id = s.Id,
                    SubjectName = s.Name
                })
                .ToArrayAsync();

            StudentRecordUpdateModel classes = new StudentRecordUpdateModel()
            {
                Students = students,
                Subjects = subjects,
                SubjectId = subjectId
            };

            return classes;
        }

        public async Task<bool> AddStudentsRecords(string userId, StudentRecordUpdateModel model)
        {
            Teacher? teacher = await _teacherRepository.FirstOrDefaultAsync(t => t.ApplicationUserId == Guid.Parse(userId));

            bool isAddedRecord = false;

            if (model.Students.All(s => s.RemarkText is null)
                 && model.Students.Any(s => s.Grade.HasValue && s.Grade is not 0)
                 && model.Students.All(s => s.IsChecked is false))
            {
                isAddedRecord = await AddGrades(userId, model, teacher);
            }
            else if (model.Students.Any(s => s.RemarkText is not null)
                         && model.Students.All(s => s.Grade is null)
                         && model.Students.All(s => s.IsChecked is false))
            {
                //isAddedRecord = await AddRemarks(userId, model);
            }
            else if (model.Students.All(s => s.RemarkText is null)
                         && model.Students.All(s => s.Grade is null)
                         && model.Students.Any(s => s.IsChecked is true))
            {
                isAddedRecord = await AddAbsence(userId, model);
            }

            return isAddedRecord;
        }

        public async Task<bool> AddGrades(string userId, StudentRecordUpdateModel model, Teacher? teacher)
        {
            List<Grade> grades = new List<Grade>();
            foreach (var student in model.Students.Where(s => s.Grade != 0))
            {
                Grade grade = new Grade()
                {
                    AddedOn = model.AddedOn,
                    GradeValue = (int)student.Grade!,
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

            await _gradeRepository.AddRangeAsync(grades);

            return true;
        }
        //public async Task<bool> AddRemarks(string userId, StudentRecordUpdateModel model)
        //{

        //}
        public async Task<bool> AddAbsence(string userId, StudentRecordUpdateModel model)
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

            await _absenceRepository.AddRangeAsync(absences);

            return true;
        }
    }
}