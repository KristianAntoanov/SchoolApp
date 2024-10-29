using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolApp.Data.Models;

namespace SchoolApp.Data.Configuration
{
    public class SubjectStudentConfiguration : IEntityTypeConfiguration<SubjectStudent>
    {
        public void Configure(EntityTypeBuilder<SubjectStudent> builder)
        {
            builder.HasOne(ss => ss.Student)
                   .WithMany(s => s.SubjectStudents)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ss => ss.Subject)
                   .WithMany(s => s.SubjectStudents)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}