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
                    ApplicationUserId = Guid.Parse("79eb351b-ed32-4309-9234-88db8555cd3d")
                },
                new Teacher()
                {
                    GuidId = Guid.NewGuid(),
                    FirstName = "Мария",
                    LastName = "Чавдарова",
                    ImageUrl = "/img/No_Image.jpg",
                    JobTitle = "Учител",
                    ApplicationUserId = Guid.Parse("e4c5fd5f-c02a-474b-8f51-d4a543f361d3")
                },
                new Teacher()
                {
                    GuidId = Guid.NewGuid(),
                    FirstName = "Ани",
                    LastName = "Григорова",
                    ImageUrl = "/img/No_Image.jpg",
                    JobTitle = "Учител",
                    ApplicationUserId = Guid.Parse("d040cb3e-ae29-4045-943c-4030a4249476")
                },
                new Teacher()
                {
                    GuidId = Guid.NewGuid(),
                    FirstName = "Цветелина",
                    LastName = "Томова",
                    ImageUrl = "/img/No_Image.jpg",
                    JobTitle = "Заместник-директор",
                    ApplicationUserId = Guid.Parse("1874d51f-29bc-4669-8f9d-938eaa55e4dd")
                },
                new Teacher()
                {
                    GuidId = Guid.NewGuid(),
                    FirstName = "Емилия",
                    LastName = "Истаткова",
                    ImageUrl = "/img/No_Image.jpg",
                    JobTitle = "Заместник-директор",
                    ApplicationUserId = Guid.Parse("eaad8ef4-d0c4-4cee-bbf0-e1f8e43a6d99")
                }
            };

            return teachers;
        }
    }
}