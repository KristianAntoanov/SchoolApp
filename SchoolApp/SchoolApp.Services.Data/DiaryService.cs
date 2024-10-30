using Microsoft.EntityFrameworkCore;
using SchoolApp.Data.Models;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Web.ViewModels;

namespace SchoolApp.Services.Data
{
	public class DiaryService
	{
		private readonly IRepository<Class, int> _classRepository;
		private readonly IRepository<Subject, int> _subjectRepository;

		public DiaryService(IRepository<Class, int> classRepository,
							IRepository<Subject, int> subjectRepository)
		{
            _classRepository = classRepository;
			_subjectRepository = subjectRepository;

        }

		public async Task<IEnumerable<DiaryIndexViewModel>> IndexGetAllClasses()
		{
			IEnumerable<DiaryIndexViewModel> diaries = await _classRepository
                .GetAllAttached()
				.OrderBy(c => c.GradeLevel)
				.ThenBy(c => c.SectionId)
				.Select(c => new DiaryIndexViewModel()
				{
					ClassName = $"{c.GradeLevel} {c.Section.Name}"
                })
				.ToArrayAsync();

			return diaries;
		}

   //     public async Task<IEnumerable<SubjectsViewModel>> GetClassContent()
   //     {
			//IEnumerable<SubjectsViewModel> subjects = await _subjectRepository
			//	.GetAllAttached()
			//	.Select(s => new SubjectsViewModel()
			//	{
			//		SubjectName = s.Name
			//	})
			//	.ToArrayAsync();

   //         return subjects;
   //     }

    }
}