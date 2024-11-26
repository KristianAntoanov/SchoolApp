using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolApp.Data.Models;

namespace SchoolApp.Data.Configuration
{
    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData(this.SeedRoles());
        }

        private List<ApplicationRole> SeedRoles()
        {
            List<ApplicationRole> roles = new List<ApplicationRole>()
            {
                new ApplicationRole()
                {
                    Id = Guid.Parse("bc1bfaec-7297-48f0-a649-f290de46ad74"),
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new ApplicationRole()
                {
                    Id = Guid.Parse("951a0b30-2bcb-4e61-b0fa-d90512119130"),
                    Name = "Parent",
                    NormalizedName = "PARENT"
                },
                new ApplicationRole()
                {
                    Id = Guid.Parse("167b9fd4-2252-4d5f-9b5d-867599a3e746"),
                    Name = "Teacher",
                    NormalizedName = "TEACHER"
                }
            };

            return roles;
        }
    }
}