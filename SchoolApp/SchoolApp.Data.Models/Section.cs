using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

using static SchoolApp.Common.EntityValidationConstants.Section;

namespace SchoolApp.Data.Models;

[Comment("Sections table")]
public class Section
{
    [Key]
    [Comment("Section identifier")]
    public int Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    [Comment("Section name")]
    public string Name { get; set; } = null!;
}