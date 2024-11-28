using SchoolApp.Web.ViewModels.News;

namespace SchoolApp.Services.Data.Contrancts
{
	public interface INewsService
	{
        Task<IEnumerable<NewsViewModel>> GetAllNewsAsync();

        Task<(bool success, string message)> AddNewsAsync(AddNewsViewModel model);

        Task<NewsViewModel?> GetNewsDetailsAsync(int id);

        Task<(bool success, string message)> DeleteNewsAsync(int id);

        Task<IEnumerable<AnnouncementViewModel>> GetAllImportantMessagesAsync();

        Task<(bool success, string message)> AddAnnouncementAsync(AddAnnouncementViewModel model);

        Task<AddAnnouncementViewModel?> GetAnnouncementForEditAsync(int id);

        Task<(bool success, string message)> EditAnnouncementAsync(int id, AddAnnouncementViewModel model);

        Task<(bool success, string message)> DeleteAnnouncementAsync(int id);

        Task<IEnumerable<NewsViewModel>> GetAllAchievementsAsync();
    }
}