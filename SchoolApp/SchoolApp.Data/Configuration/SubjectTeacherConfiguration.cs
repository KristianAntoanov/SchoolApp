using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SchoolApp.Data.Models;

namespace SchoolApp.Data.Configuration
{
    public class SubjectTeacherConfiguration : IEntityTypeConfiguration<SubjectTeacher>
    {
        public void Configure(EntityTypeBuilder<SubjectTeacher> builder)
        {
            builder.HasOne(st => st.Teacher)
                   .WithMany(t => t.SubjectTeachers)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(st => st.Subject)
                   .WithMany(s => s.SubjectTeachers)
                   .OnDelete(DeleteBehavior.Restrict);

            //builder.HasData(this.SeedSubjectsTeachers());
        }

        //private List<SubjectTeacher> SeedSubjectsTeachers()
        //{
        //    List<SubjectTeacher> subjectsTeachers = new List<SubjectTeacher>()
        //    {
        //        new SubjectTeacher()
        //        {
        //            TeacherId = Guid.Parse("318f53a8-f263-471b-af91-505d5b3e5bce"),
        //            SubjectId = 2
        //        },
        //        new SubjectTeacher()
        //        {
        //            TeacherId = Guid.Parse("318f53a8-f263-471b-af91-505d5b3e5bce"),
        //            SubjectId = 3
        //        },
        //        new SubjectTeacher()
        //        {
        //            TeacherId = Guid.Parse("4bbc8967-7e28-45c9-a404-3e0c46674d4e"),
        //            SubjectId = 1
        //        },
        //        new SubjectTeacher()
        //        {
        //            TeacherId = Guid.Parse("fbfc8ff1-27a2-40df-97ed-d38012b3424f"),
        //            SubjectId = 4
        //        },
        //        new SubjectTeacher()
        //        {
        //            TeacherId = Guid.Parse("078b0356-6f4a-445a-aa7d-474dd5503f7a"),
        //            SubjectId = 5
        //        },
        //        new SubjectTeacher()
        //        {
        //            TeacherId = Guid.Parse("2275c32d-412a-4eb4-9e41-ea45be7ad9f9"),
        //            SubjectId = 6
        //        },
        //        new SubjectTeacher()
        //        {
        //            TeacherId = Guid.Parse("6dd73b5a-c6c6-4ef6-a919-e88c9cdda2fc"),
        //            SubjectId = 5
        //        },
        //    };

        //    return subjectsTeachers;
        //}
    }
}