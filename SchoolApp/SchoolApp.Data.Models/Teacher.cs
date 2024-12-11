using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using static SchoolApp.Common.EntityValidationConstants.Teacher;

namespace SchoolApp.Data.Models;

[Comment("Teachers table")]
public class Teacher
{
    [Key]
    [Comment("Teacher identifier")]
    public Guid GuidId { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    [Comment("Teacher first name")]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(NameMaxLength)]
    [Comment("Teacher last name")]
    public string LastName { get; set; } = null!;

    [Required]
    [MaxLength(ImageUrlMaxLength)]
    [Comment("Image URL")]
    public string ImageUrl { get; set; } = null!;

    [Required]
    [MaxLength(JobTitleMaxLength)]
    [Comment("Job title")]
    public string JobTitle { get; set; } = null!;

    [Comment("Application user identifier")]
    public Guid? ApplicationUserId { get; set; }

    [ForeignKey(nameof(ApplicationUserId))]
    public virtual ApplicationUser? ApplicationUser { get; set; }

    public virtual ICollection<SubjectTeacher> SubjectTeachers { get; set; }
        = new HashSet<SubjectTeacher>();
}