using System.ComponentModel.DataAnnotations;

using static SchoolApp.Common.EntityValidationConstants.Album;

namespace SchoolApp.Data.Models
{
	public class Album
	{
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        public ICollection<GalleryImage> Images { get; set; }
            = new HashSet<GalleryImage>();
    }
}