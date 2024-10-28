using System.ComponentModel.DataAnnotations;

namespace SchoolApp.Infrastructure.Data.Models
{
	public class Section
	{
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(1)]
        public string Name { get; set; } = null!;
    }
}