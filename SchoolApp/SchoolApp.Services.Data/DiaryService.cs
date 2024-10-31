using Microsoft.EntityFrameworkCore;
using SchoolApp.Data.Models;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Web.ViewModels;

namespace SchoolApp.Services.Data
{
	public class DiaryService
	{
		private readonly IRepository<Class, int> _classRepository;
		private readonly IRepository<Student, int> _studentRepository;

		public DiaryService(IRepository<Class, int> classRepository,
							IRepository<Student, int> studentRepository)
		{
            _classRepository = classRepository;
            _studentRepository = studentRepository;

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

		public async Task<IEnumerable<StudentsViewModel>> GetClassContent(int classId, int subjectId)
		{
			IEnumerable<StudentsViewModel> students = await _studentRepository
				.GetAllAttached()
				.Where(s => s.ClassId == classId)
				.Select(s => new StudentsViewModel()
				{
					FirstName = s.FirstName,
					LastName = s.LastName,
					Grades = s.Grades
								.Where(g => g.SubjectId == subjectId)
								.Select(g => new GradeViewModel()
								{
									GradeValue = g.GradeValue
								})
								.ToArray()
				})
				.ToArrayAsync();

			return students;
		}

	}
}