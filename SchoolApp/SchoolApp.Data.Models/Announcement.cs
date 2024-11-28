using System.ComponentModel.DataAnnotations;

using static SchoolApp.Common.EntityValidationConstants.Announcement;

namespace SchoolApp.Data.Models
{
	public class Announcement
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
    }
}