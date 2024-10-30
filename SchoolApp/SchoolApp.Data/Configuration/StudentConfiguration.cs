using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SchoolApp.Data.Models;

namespace SchoolApp.Data.Configuration
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasOne(s => s.Class)
                   .WithMany(c => c.Students)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(this.SeedStudents());
        }

        private List<Student> SeedStudents()
        {
            List<Student> students = new List<Student>()
            {
                new Student()
                {
                    Id = 1,
                    FirstName = "Иван",
                    MiddleName = "Неделинов",
                    LastName = "Иванов",
                    ClassId = 1,
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"),
                },
                new Student()
                {
                    Id = 2,
                    FirstName = "Мария",
                    MiddleName = "Викторова",
                    LastName = "Петрова",
                    ClassId = 1,
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"),
                },
                new Student()
                {
                    Id = 3,
                    FirstName = "Георги",
                    MiddleName = "Петрунов",
                    LastName = "Димитров",
                    ClassId = 1,
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"),
                },
                new Student()
                {
                    Id = 4,
                    FirstName = "Елена",
                    MiddleName = "Георгиева",
                    LastName = "Станкова",
                    ClassId = 2,
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"),
                },
                new Student()
                {
                    Id = 5,
                    FirstName = "Петра",
                    MiddleName = "Петрунова",
                    LastName = "Стайкова",
                    ClassId = 2,
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"),
                },
                new Student()
                {
                    Id = 6,
                    FirstName = "Георги",
                    MiddleName = "Иванов",
                    LastName = "Петров",
                    ClassId = 2,
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"),
                },
                new Student()
                {
                    Id = 7,
                    FirstName = "Мария",
                    MiddleName = "Георгиева",
                    LastName = "Петрова",
                    ClassId = 3,
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"),
                },
                new Student()
                {
                    Id = 8,
                    FirstName = "Иван",
                    MiddleName = "Стефанов",
                    LastName = "Иванов",
                    ClassId = 3,
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"),
                },
                new Student()
                {
                    Id = 9,
                    FirstName = "Елена",
                    MiddleName = "Николова",
                    LastName = "Василева",
                    ClassId = 3,
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"),
                },
                new Student()
                {
                    Id = 10,
                    FirstName = "Никола",
                    MiddleName = "Петров",
                    LastName = "Димитров",
                    ClassId = 4,
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"),
                },
                new Student()
                {
                    Id = 11,
                    FirstName = "Петър",
                    MiddleName = "Георгиев",
                    LastName = "Димитров",
                    ClassId = 4,
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"),
                },
                new Student()
                {
                    Id = 12,
                    FirstName = "Даниела",
                    MiddleName = "Иванова",
                    LastName = "Маринова",
                    ClassId = 4,
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"),
                },
                new Student()
                {
                    Id = 13,
                    FirstName = "Александър",
                    MiddleName = "Николов",
                    LastName = "Стоянов",
                    ClassId = 5,
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"),
                },
                new Student()
                {
                    Id = 14,
                    FirstName = "Калина",
                    MiddleName = "Петкова",
                    LastName = "Димитрова",
                    ClassId = 5,
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"),
                },
                new Student()
                {
                    Id = 15,
                    FirstName = "Радослав",
                    MiddleName = "Георгиев",
                    LastName = "Петров",
                    ClassId = 5,
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"),
                },
                new Student()
                {
                    Id = 16,
                    FirstName = "Борис",
                    MiddleName = "Стефанов",
                    LastName = "Караджов",
                    ClassId = 6,
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"),
                },
                new Student()
                {
                    Id = 17,
                    FirstName = "Антония",
                    MiddleName = "Илиева",
                    LastName = "Тодорова",
                    ClassId = 6,
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"),
                },
                new Student()
                {
                    Id = 18,
                    FirstName = "Виктор",
                    MiddleName = "Алексиев",
                    LastName = "Колев",
                    ClassId = 6,
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"),
                }

            };

            return students;
        }
    }
}