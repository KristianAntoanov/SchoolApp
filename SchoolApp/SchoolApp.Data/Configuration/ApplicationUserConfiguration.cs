using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SchoolApp.Data.Models;

namespace SchoolApp.Data.Configuration;

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
                UserName = "Stefan@gmail.com",
                NormalizedUserName = "STEFAN@GMAIL.COM",
                Email = "Stefan@gmail.com",
                NormalizedEmail = "STEFAN@GMAIL.COM",
                SecurityStamp = Guid.NewGuid().ToString().ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                AccessFailedCount = 0,
                LockoutEnabled = true,
                PasswordHash = hasher.HashPassword(null, "Ss123456")
            },
            new ApplicationUser()
            {
                Id = Guid.Parse("79eb351b-ed32-4309-9234-88db8555cd3d"),
                UserName = "Margarita@gmail.com",
                NormalizedUserName = "MARGARITA@GMAIL.COM",
                Email = "Margarita@gmail.com",
                NormalizedEmail = "MARGARITA@GMAIL.COM",
                SecurityStamp = Guid.NewGuid().ToString().ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                AccessFailedCount = 0,
                LockoutEnabled = true,
                PasswordHash = hasher.HashPassword(null, "Mm123456")
            },
            new ApplicationUser()
            {
                Id = Guid.Parse("e4c5fd5f-c02a-474b-8f51-d4a543f361d3"),
                UserName = "Maria@gmail.com",
                NormalizedUserName = "MARIA@GMAIL.COM",
                Email = "Maria@gmail.com",
                NormalizedEmail = "MARIA@GMAIL.COM",
                SecurityStamp = Guid.NewGuid().ToString().ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                AccessFailedCount = 0,
                LockoutEnabled = true,
                PasswordHash = hasher.HashPassword(null, "Mch123456")
            },
            new ApplicationUser()
            {
                Id = Guid.Parse("d040cb3e-ae29-4045-943c-4030a4249476"),
                UserName = "Ani@gmail.com",
                NormalizedUserName = "ANI@GMAIL.COM",
                Email = "Ani@gmail.com",
                NormalizedEmail = "ANI@GMAIL.COM",
                SecurityStamp = Guid.NewGuid().ToString().ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                AccessFailedCount = 0,
                LockoutEnabled = true,
                PasswordHash = hasher.HashPassword(null, "Aa123456")
            },
            new ApplicationUser()
            {
                Id = Guid.Parse("1874d51f-29bc-4669-8f9d-938eaa55e4dd"),
                UserName = "Tsveti@gmail.com",
                NormalizedUserName = "TSVETI@GMAIL.COM",
                Email = "Tsveti@gmail.com",
                NormalizedEmail = "TSVETI@GMAIL.COM",
                SecurityStamp = Guid.NewGuid().ToString().ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                AccessFailedCount = 0,
                LockoutEnabled = true,
                PasswordHash = hasher.HashPassword(null, "Cc123456")
            },
            new ApplicationUser()
            {
                Id = Guid.Parse("eaad8ef4-d0c4-4cee-bbf0-e1f8e43a6d99"),
                UserName = "Emilia@gmail.com",
                NormalizedUserName = "EMILIA@GMAIL.COM",
                Email = "Emilia@gmail.com",
                NormalizedEmail = "EMILIA@GMAIL.COM",
                SecurityStamp = Guid.NewGuid().ToString().ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                AccessFailedCount = 0,
                LockoutEnabled = true,
                PasswordHash = hasher.HashPassword(null, "Ee123456")
            }
        };

        return users;
    }
}