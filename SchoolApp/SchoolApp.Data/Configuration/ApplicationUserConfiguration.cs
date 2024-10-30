using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SchoolApp.Data.Models;

namespace SchoolApp.Data.Configuration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasData(this.SeedUsers());
        }

        private List<ApplicationUser> SeedUsers()
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            List<ApplicationUser> users = new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    Id = Guid.Parse("39d7bb6d-eb8a-40fc-be00-9c5296a2717c"),
                    UserName = "Peshko@abv.com",
                    NormalizedUserName = "PESHKO@ABV.COM",
                    Email = "Peshko@abv.com",
                    NormalizedEmail = "PESHKO@ABV.COM",
                    SecurityStamp = Guid.NewGuid().ToString().ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    TwoFactorEnabled = false,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    AccessFailedCount = 0,
                    LockoutEnabled = true,
                    PasswordHash = hasher.HashPassword(null, "Aa123456")
                }
            };

            return users;
        }
    }
}