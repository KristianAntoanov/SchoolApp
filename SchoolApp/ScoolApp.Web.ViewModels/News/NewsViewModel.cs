using SchoolApp.Data.Models;

namespace SchoolApp.Web.ViewModels.News;

public class NewsViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime PublicationDate { get; set; }

    public string? ImageUrl { get; set; }

    public NewsCategory Category { get; set; }
}