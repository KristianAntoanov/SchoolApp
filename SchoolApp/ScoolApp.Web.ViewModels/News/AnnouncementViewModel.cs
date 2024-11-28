namespace SchoolApp.Web.ViewModels.News
{
	public class AnnouncementViewModel
	{
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime PublicationDate { get; set; }
    }
}