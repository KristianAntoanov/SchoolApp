using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

using static SchoolApp.Common.EntityValidationConstants.Subject;

namespace SchoolApp.Data.Models;

[Comment("Subjects table")]
public class Subject
{
    [Key]
    [Comment("Subject identifier")]
    public int Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    [Comment("Subject name")]
    public string Name { get; set; } = null!;

    public virtual ICollection<SubjectStudent> SubjectStudents { get; set; }
        = new HashSet<SubjectStudent>();

    public virtual ICollection<SubjectTeacher> SubjectTeachers { get; set; }
        = new HashSet<SubjectTeacher>();
}