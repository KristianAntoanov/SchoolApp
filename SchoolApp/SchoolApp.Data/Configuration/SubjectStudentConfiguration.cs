using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SchoolApp.Data.Models;

namespace SchoolApp.Data.Configuration;

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

        builder.HasData(this.SeedSubjectsStudents());
    }

    private List<SubjectStudent> SeedSubjectsStudents()
    {
        List<SubjectStudent> subjectsStudents = new List<SubjectStudent>();

        for (int studentId = 1; studentId <= 18; studentId++)
        {
            for (int subjectId = 1; subjectId <= 6; subjectId++)
            {
                subjectsStudents.Add(new SubjectStudent
                {
                    StudentId = studentId,
                    SubjectId = subjectId
                });
            }
        }
        return subjectsStudents;
    }
}