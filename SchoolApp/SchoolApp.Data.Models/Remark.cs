using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using static SchoolApp.Common.EntityValidationConstants.Remark;

namespace SchoolApp.Data.Models;

[Comment("Remarks table")]
public class Remark
{
    [Key]
    [Comment("Remark identifier")]
    public int Id { get; set; }

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

    [Comment("Teacher identifier")]
    public Guid? TeacherId { get; set; }

    [ForeignKey(nameof(TeacherId))]
    public Teacher Teacher { get; set; } = null!;

    [Required]
    [MaxLength(RemarkTextMaxLength)]
    [Comment("Remark text")]
    public string RemarkText { get; set; } = null!;

    [Required]
    [Comment("Date of creation")]
    public DateTime AddedOn { get; set; }
}