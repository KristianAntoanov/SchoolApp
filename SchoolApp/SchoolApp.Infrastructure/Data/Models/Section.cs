using System.ComponentModel.DataAnnotations;

using static SchoolApp.Common.EntityValidationConstants.Section;

namespace SchoolApp.Infrastructure.Data.Models
{
	public class Section
	{
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;
    }
}