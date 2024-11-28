using System.ComponentModel.DataAnnotations;

using static SchoolApp.Common.EntityValidationConstants.News;

namespace SchoolApp.Data.Models
{
	public class News
	{
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; } = null!;

        [Required]
        public DateTime PublicationDate { get; set; }

        [MaxLength(ImageUrlMaxLength)]
        public string? ImageUrl { get; set; }

        public bool IsArchived { get; set; }

        [Required]
        public NewsCategory Category { get; set; }
    }
}