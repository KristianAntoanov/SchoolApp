using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace SchoolApp.Data.Models;

[PrimaryKey(nameof(TeacherId), nameof(SubjectId))]
[Comment("Subject teachers mapping table")]
public class SubjectTeacher
{
    [Required]
    [Comment("Teacher identifier")]
    public Guid TeacherId { get; set; }

    [ForeignKey(nameof(TeacherId))]
    public Teacher Teacher { get; set; } = null!;

    [Required]
    [Comment("Subject identifier")]
    public int SubjectId { get; set; }

    [ForeignKey(nameof(SubjectId))]
    public Subject Subject { get; set; } = null!;
}