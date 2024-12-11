using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace SchoolApp.Data.Models;

[PrimaryKey(nameof(StudentId), nameof(SubjectId))]
[Comment("Subject students mapping table")]
public class SubjectStudent
{
    [Required]
    [Comment("Student identifier")]
    public int StudentId { get; set; }

    [ForeignKey(nameof(StudentId))]
    public Student Student { get; set; } = null!;

    [Required]
    [Comment("Subject identifier")]
    public int SubjectId { get; set; }

    [ForeignKey(nameof(SubjectId))]
    public Subject Subject { get; set; } = null!;
}