using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using static SchoolApp.Common.EntityValidationConstants.GalleryImage;

namespace SchoolApp.Data.Models;

[Comment("Gallery images table")]
public class GalleryImage
{
    [Key]
    [Comment("Image identifier")]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(ImageUrlMaxLength)]
    [Comment("Image URL")]
    public string ImageUrl { get; set; } = null!;

    [Required]
    [Comment("Album identifier")]
    public Guid AlbumId { get; set; }

    [ForeignKey(nameof(AlbumId))]
    public Album Album { get; set; } = null!;
}