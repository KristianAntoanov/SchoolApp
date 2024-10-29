using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SchoolApp.Data.Models;

namespace SchoolApp.Data.Configuration
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.HasData(this.SeedMovies());
        }

        private List<Subject> SeedMovies()
        {
            List<Subject> subjects = new List<Subject>()
            {
                new Subject()
                {
                    Id = 1,
                    Name = "Български език и литература"
                },
                new Subject()
                {
                    Id = 2,
                    Name = "Математика"
                },
                new Subject()
                {
                    Id = 3,
                    Name = "Физика"
                },
                new Subject()
                {
                    Id = 4,
                    Name = "Химия"
                },
                new Subject()
                {
                    Id = 5,
                    Name = "Програмиране"
                },
                new Subject()
                {
                    Id = 6,
                    Name = "История"
                }
            };

            return subjects;
        }
    }
}