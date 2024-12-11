using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SchoolApp.Data.Models;

namespace SchoolApp.Data.Configuration;

public class GradeConfiguration : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.HasOne(g => g.Teacher)
               .WithMany()
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(g => g.GradeType)
               .HasDefaultValue(GradeType.Current)
               .IsRequired();
    }
}