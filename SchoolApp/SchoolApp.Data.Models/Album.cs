using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

using static SchoolApp.Common.EntityValidationConstants.Album;

namespace SchoolApp.Data.Models;

[Comment("Albums table")]
public class Album
{
    [Key]
    [Comment("Album identifier")]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(TitleMaxLength)]
    [Comment("Album title")]
    public string Title { get; set; } = null!;

    [MaxLength(DescriptionMaxLength)]
    [Comment("Album description")]
    public string? Description { get; set; }

    public ICollection<GalleryImage> Images { get; set; }
        = new HashSet<GalleryImage>();
}