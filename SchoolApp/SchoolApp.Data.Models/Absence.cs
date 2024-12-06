using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace SchoolApp.Data.Models;

[Comment("Absences table")]
public class Absence
{
    [Key]
    [Comment("Absence identifier")]
    public int Id { get; set; }

    [Required]
    [Comment("Student identifier")]
    public int StudentId { get; set; }

    [ForeignKey(nameof(StudentId))]
    public virtual Student Student { get; set; } = null!;

    [Required]
    [Comment("Subject identifier")]
    public int SubjectId { get; set; }

    [ForeignKey(nameof(SubjectId))]
    public virtual Subject Subject { get; set; } = null!;

    [Required]
    [Comment("Absence status")]
    public bool IsExcused { get; set; }

    [Required]
    [Comment("Date of creation")]
    public DateTime AddedOn { get; set; }
}