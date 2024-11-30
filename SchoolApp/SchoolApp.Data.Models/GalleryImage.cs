using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static SchoolApp.Common.EntityValidationConstants.GalleryImage;

namespace SchoolApp.Data.Models
{
	public class GalleryImage
	{
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(ImageUrlMaxLength)]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public Guid AlbumId { get; set; }

        [ForeignKey(nameof(AlbumId))]
        public Album Album { get; set; } = null!;
    }
}