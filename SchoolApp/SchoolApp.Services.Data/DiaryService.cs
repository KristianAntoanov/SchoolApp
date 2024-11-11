using Microsoft.EntityFrameworkCore;

using SchoolApp.Data.Models;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels;
using SchoolApp.Web.ViewModels.Diary.NewTest;

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
        private readonly IRepository<Remark, int> _remarkRepository;

        public DiaryService(IRepository<Class, int> classRepository,
                            IRepository<Student, int> studentRepository,
                            IRepository<Subject, int> subjectRepository,
                            IRepository<Grade, int> gradeRepository,
                            IRepository<Teacher, Guid> teacherRepository,
                            IRepository<Absence, int> absenceRepository,
                            IRepository<Remark, int> remarkRepository)
        {
            _classRepository = classRepository;
            _studentRepository = studentRepository;
            _subjectRepository = subjectRepository;
            _gradeRepository = gradeRepository;
            _teacherRepository = teacherRepository;
            _absenceRepository = absenceRepository;
            _remarkRepository = remarkRepository;
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

        public async Task<T> GetClassStudentForGrades<T>(int classId, int subjectId)
            where T : StudentBaseViewModel, new()
        {
            T model = new()
            {
                AddedOn = DateTime.Now,
                SubjectId = subjectId,
                Subjects = await _subjectRepository
            .GetAllAttached()
            .Select(s => new SubjectViewModel()
            {
                Id = s.Id,
                Name = s.Name
            })
            .ToArrayAsync()
            };

            var students = await _studentRepository
                .GetAllAttached()
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
            Teacher? teacher = await _teacherRepository.FirstOrDefaultAsync(t => t.ApplicationUserId == Guid.Parse(userId));

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

            await _absenceRepository.AddRangeAsync(absences);

            return true;
        }
        public async Task<bool> AddRemark(string userId, RemarkFormModel model)
        {
            Teacher? teacher = await _teacherRepository.FirstOrDefaultAsync(t => t.ApplicationUserId == Guid.Parse(userId));

            Remark remark = new Remark()
            {
                AddedOn = model.AddedOn,
                RemarkText = model.RemarkText,
                StudentId = model.StudentId,
                SubjectId = model.SubjectId,
                TeacherId = teacher!.GuidId
            };

            await _remarkRepository.AddAsync(remark);

            return true;
        }
    }
}