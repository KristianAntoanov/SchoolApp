using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolApp.Data.Models;

namespace SchoolApp.Data.Configuration
{
    public class ClassConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.HasData(this.SeedClasses());
        }

        private List<Class> SeedClasses()
        {
            List<Class> classes = new List<Class>()
            {
                new Class()
                {
                    Id = 1,
                    GradeLevel = 5,
                    SectionId = 1
                },
                new Class()
                {
                    Id = 2,
                    GradeLevel = 5,
                    SectionId = 2
                },
                new Class()
                {
                    Id = 3,
                    GradeLevel = 5,
                    SectionId = 3
                },
                new Class()
                {
                    Id = 4,
                    GradeLevel = 6,
                    SectionId = 1
                },
                new Class()
                {
                    Id = 5,
                    GradeLevel = 6,
                    SectionId = 2
                },
                new Class()
                {
                    Id = 6,
                    GradeLevel = 6,
                    SectionId = 3
                }
            };
            return classes;
        }
    }
}