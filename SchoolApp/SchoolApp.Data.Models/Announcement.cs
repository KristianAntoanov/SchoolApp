using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

using static SchoolApp.Common.EntityValidationConstants.Announcement;

namespace SchoolApp.Data.Models;

[Comment("Announcements table")]
public class Announcement
{
    [Key]
    [Comment("Announcement identifier")]
    public int Id { get; set; }

    [Required]
    [MaxLength(TitleMaxLength)]
    [Comment("Announcement title")]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(ContentMaxLength)]
    [Comment("Announcement content")]
    public string Content { get; set; } = null!;

    [Required]
    [Comment("Announcement publication date")]
    public DateTime PublicationDate { get; set; }
}