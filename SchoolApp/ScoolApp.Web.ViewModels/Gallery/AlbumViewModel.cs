namespace SchoolApp.Web.ViewModels.Gallery;

public class AlbumViewModel
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public IList<GalleryImageViewModel> Images { get; set; }
        = new List<GalleryImageViewModel>();
}