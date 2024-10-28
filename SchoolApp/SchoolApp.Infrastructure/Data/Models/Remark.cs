using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static SchoolApp.Common.EntityValidationConstants.Remark;

namespace SchoolApp.Infrastructure.Data.Models
{
	public class Remark
	{
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; } = null!;

        [Required]
        public int SubjectId { get; set; }

        [ForeignKey(nameof(SubjectId))]
        public Subject Subject { get; set; } = null!;

        [Required]
        [MaxLength(RemarkTextMaxLength)]
        public string RemarkText { get; set; } = null!;

        [Required]
        public DateTime AddedOn { get; set; }
    }
}