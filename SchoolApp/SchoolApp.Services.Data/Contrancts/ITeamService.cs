using SchoolApp.Web.ViewModels.Team;

namespace SchoolApp.Services.Data.Contrancts;

public interface ITeamService
{
    Task<IEnumerable<TeachersViewModel>> GetAllTeachers();
}