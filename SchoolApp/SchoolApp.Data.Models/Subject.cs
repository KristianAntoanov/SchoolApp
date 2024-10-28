using System.ComponentModel.DataAnnotations;

using static SchoolApp.Common.EntityValidationConstants.Subject;

namespace SchoolApp.Data.Models
{
	public class Subject
	{
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Grade> Grades { get; set; }
            = new HashSet<Grade>();

        public virtual ICollection<Remark> Remarks { get; set; }
            = new HashSet<Remark>();

        public virtual ICollection<Absence> Absences { get; set; }
            = new HashSet<Absence>();

        public virtual ICollection<SubjectStudent> SubjectStudents { get; set; }
            = new HashSet<SubjectStudent>();

        public virtual ICollection<SubjectTeacher> SubjectTeachers { get; set; }
            = new HashSet<SubjectTeacher>();
    }
}