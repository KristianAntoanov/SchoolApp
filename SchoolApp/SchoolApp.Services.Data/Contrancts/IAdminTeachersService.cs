using SchoolApp.Web.ViewModels.Admin.Teachers;

namespace SchoolApp.Services.Data.Contrancts
{
	public interface IAdminTeachersService
	{
        Task<IEnumerable<TeacherViewModel>> GetAllTeachersAsync();
    }
}