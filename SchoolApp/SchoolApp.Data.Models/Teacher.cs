using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static SchoolApp.Common.EntityValidationConstants.Teacher;

namespace SchoolApp.Data.Models
{
	public class Teacher
	{
        [Key]
        public Guid GuidId { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(NameMaxLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [MaxLength(ImageUrlMaxLength)]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [MaxLength(JobTitleMaxLength)]
        public string JobTitle { get; set; } = null!;

        [Required]
        public Guid ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        public virtual ICollection<SubjectTeacher> SubjectTeachers { get; set; }
            = new HashSet<SubjectTeacher>();
    }
}