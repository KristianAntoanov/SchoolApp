using Microsoft.EntityFrameworkCore;
using SchoolApp.Data.Models;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels;

namespace SchoolApp.Services.Data
{
	public class DiaryService : IDiaryService
    {
		private readonly IRepository<Class, int> _classRepository;
		private readonly IRepository<Student, int> _studentRepository;
		private readonly IRepository<Subject, int> _subjectRepository;
		private readonly IRepository<Grade, int> _gradeRepository;

		public DiaryService(IRepository<Class, int> classRepository,
							IRepository<Student, int> studentRepository,
                            IRepository<Subject, int> subjectRepository,
                            IRepository<Grade, int> gradeRepository)
		{
            _classRepository = classRepository;
            _studentRepository = studentRepository;
			_subjectRepository = subjectRepository;
            _gradeRepository = gradeRepository;
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

        public async Task<DiaryGradeAddViewModel> GetClassNames(int classId, int subjectId)
        {
            IList<StudentVewModel> students = await _studentRepository
                .GetAllAttached()
                .Where(s => s.ClassId == classId)
                .Select(s => new StudentVewModel()
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName
                })
                .ToListAsync();

            IEnumerable<SubjectsViewModel> subjects = await _subjectRepository
                .GetAllAttached()
                .Select(s => new SubjectsViewModel()
                {
                    Id = s.Id,
                    SubjectName = s.Name
                })
                .ToArrayAsync();

            DiaryGradeAddViewModel classes = new DiaryGradeAddViewModel()
            {
                Students = students,
                Subjects = subjects,
                SubjectId = subjectId
            };

            return classes;
        }

        public async Task<bool> AddGradesToStudents(int classId, DiaryGradeAddViewModel model)
        {
            foreach (var student in model.Students)
            {
                Grade grade = new Grade()
                {
                    AddedOn = model.AddedOn,
                    GradeValue = student.Grade,
                    StudentId = student.Id,
                    SubjectId = model.SubjectId
                };
            }
            return true;
        }

    }
}