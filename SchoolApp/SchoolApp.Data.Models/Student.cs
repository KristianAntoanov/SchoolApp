using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using static SchoolApp.Common.EntityValidationConstants.Student;

namespace SchoolApp.Data.Models;

[Comment("Students table")]
public class Student
{
    [Key]
    [Comment("Student identifier")]
    public int Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    [Comment("Student first name")]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(NameMaxLength)]
    [Comment("Student middle name")]
    public string MiddleName { get; set; } = null!;

    [Required]
    [MaxLength(NameMaxLength)]
    [Comment("Student last name")]
    public string LastName { get; set; } = null!;

    [Required]
    [Comment("Class identifier")]
    public int ClassId { get; set; }

    [ForeignKey(nameof(ClassId))]
    public Class Class { get; set; } = null!;

    [Required]
    [Comment("Application user identifier")]
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