using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

using static SchoolApp.Common.EntityValidationConstants.News;

namespace SchoolApp.Data.Models;

[Comment("News table")]
public class News
{
    [Key]
    [Comment("News identifier")]
    public int Id { get; set; }

    [Required]
    [MaxLength(TitleMaxLength)]
    [Comment("News title")]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(ContentMaxLength)]
    [Comment("News content")]
    public string Content { get; set; } = null!;

    [Required]
    [Comment("Publication date")]
    public DateTime PublicationDate { get; set; }

    [MaxLength(ImageUrlMaxLength)]
    [Comment("Image URL")]
    public string? ImageUrl { get; set; }

    [Comment("Archive status")]
    public bool IsArchived { get; set; }

    [Required]
    [Comment("News category")]
    public NewsCategory Category { get; set; }
}