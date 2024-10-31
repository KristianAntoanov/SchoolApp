using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SchoolApp.Data.Models;

namespace SchoolApp.Data.Configuration
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasData(this.SeedTeacher());
        }

        private List<Teacher> SeedTeacher()
        {
            List<Teacher> teachers = new List<Teacher>()
            {
                new Teacher()
                {
                    GuidId = Guid.NewGuid(),
                    FirstName = "Стефан",
                    LastName = "Николов",
                    ImageUrl = "/img/No_Image.jpg",
                    JobTitle = "Учител",
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c")
                },
                new Teacher()
                {
                    GuidId = Guid.NewGuid(),
                    FirstName = "Маргарита",
                    LastName = "Йорданова",
                    ImageUrl = "/img/No_Image.jpg",
                    JobTitle = "Учител",
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c")
                },
                new Teacher()
                {
                    GuidId = Guid.NewGuid(),
                    FirstName = "Мария",
                    LastName = "Чавдарова",
                    ImageUrl = "/img/No_Image.jpg",
                    JobTitle = "Учител",
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c")
                },
                new Teacher()
                {
                    GuidId = Guid.NewGuid(),
                    FirstName = "Ани",
                    LastName = "Григорова",
                    ImageUrl = "/img/No_Image.jpg",
                    JobTitle = "Учител",
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c")
                },
                new Teacher()
                {
                    GuidId = Guid.NewGuid(),
                    FirstName = "Цветелина",
                    LastName = "Томова",
                    ImageUrl = "/img/No_Image.jpg",
                    JobTitle = "Заместник-директор",
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c")
                },
                new Teacher()
                {
                    GuidId = Guid.NewGuid(),
                    FirstName = "Емилия",
                    LastName = "Истаткова",
                    ImageUrl = "/img/No_Image.jpg",
                    JobTitle = "Заместник-директор",
                    ApplicationUserId = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c")
                }
            };

            return teachers;
        }
    }
}