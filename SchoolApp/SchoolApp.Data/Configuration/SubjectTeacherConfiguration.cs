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
        }
    }
}