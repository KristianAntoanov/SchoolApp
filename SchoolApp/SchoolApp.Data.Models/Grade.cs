using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace SchoolApp.Data.Models;

[Comment("Grades table")]
public class Grade
{
    [Key]
    [Comment("Grade identifier")]
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

    [Comment("Teacher identifier")]
    public Guid? TeacherId { get; set; }

    [ForeignKey(nameof(TeacherId))]
    public Teacher Teacher { get; set; } = null!;

    [Required]
    [Comment("Grade value")]
    public int GradeValue { get; set; }

    [Required]
    [Comment("Grade type")]
    public GradeType? GradeType { get; set; }

    [Required]
    [Comment("Date of creation")]
    public DateTime AddedOn { get; set; }
}