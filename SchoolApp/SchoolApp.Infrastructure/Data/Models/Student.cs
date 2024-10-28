using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Infrastructure.Data.Models
{
	public class Student
	{
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string MiddleName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [Required]
        public int ClassId { get; set; }

        [ForeignKey(nameof(ClassId))]
        public Class Class { get; set; } = null!;

        [Required]
        public Guid ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        public virtual ICollection<Remark> Remarks { get; set; }
            = new HashSet<Remark>();

        public virtual ICollection<Absence> Absences { get; set; }
            = new HashSet<Absence>();

        public virtual ICollection<SubjectStudent> SubjectStudents { get; set; }
            = new HashSet<SubjectStudent>();

        public virtual ICollection<Grade> Grades { get; set; }
            = new HashSet<Grade>();
    }
}