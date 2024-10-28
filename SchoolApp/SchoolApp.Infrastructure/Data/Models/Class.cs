using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Infrastructure.Data.Models
{
	public class Class
	{
        [Key]
        public int Id { get; set; }

        [Required]
        public int GradeLevel { get; set; }

        [Required]
        public int SectionId { get; set; }

        [ForeignKey(nameof(SectionId))]
        public virtual Section Section { get; set; } = null!;

        public virtual ICollection<Student> Students { get; set; }
            = new HashSet<Student>();
    }
}