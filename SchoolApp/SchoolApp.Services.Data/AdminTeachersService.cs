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

        public AdminTeachersService(IRepository repository)
		{
			_repository = repository;
		}

        public async Task<IEnumerable<TeacherViewModel>> GetAllTeachersAsync()
        {
            IEnumerable<TeacherViewModel> model = await _repository
                .GetAllAttached<Teacher>()
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

    }
}