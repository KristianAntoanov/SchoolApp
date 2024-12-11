using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace SchoolApp.Data.Models;

[Comment("Classes table")]
public class Class
{
    [Key]
    [Comment("Class identifier")]
    public int Id { get; set; }

    [Required]
    [Comment("Grade level")]
    public int GradeLevel { get; set; }

    [Required]
    [Comment("Section identifier")]
    public int SectionId { get; set; }

    [ForeignKey(nameof(SectionId))]
    public virtual Section Section { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; }
        = new HashSet<Student>();
}