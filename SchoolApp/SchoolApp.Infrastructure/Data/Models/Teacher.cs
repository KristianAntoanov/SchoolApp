using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Infrastructure.Data.Models
{
	public class Teacher
	{
        [Key]
        public Guid GuidId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string JobTitle { get; set; } = null!;

        [Required]
        public Guid ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        public virtual ICollection<SubjectTeacher> SubjectTeachers { get; set; }
            = new HashSet<SubjectTeacher>();
    }
}