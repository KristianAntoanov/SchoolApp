using Microsoft.EntityFrameworkCore;

using SchoolApp.Data.Models;
using SchoolApp.Data.Repository.Contracts;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.Team;

namespace SchoolApp.Services.Data;

public class TeamService : ITeamService
{
    private readonly IRepository _repository;

    public TeamService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TeachersViewModel>> GetAllTeachers()
    {
        IEnumerable<TeachersViewModel> teachers = await _repository
            .GetAllAttached<Teacher>()
            .Include(st => st.SubjectTeachers)
            .Select(t => new TeachersViewModel()
            {
                FirstName = t.FirstName,
                LastName = t.LastName,
                JobTitle = t.JobTitle,
                Photo = t.ImageUrl,
                Subjects = t.SubjectTeachers.Select(st => new TeacherSubjectsViewModel()
                {
                    Id = st.SubjectId,
                    Name = st.Subject.Name
                })
                .ToArray()
            })
            .ToArrayAsync();

        return teachers;
    }
}