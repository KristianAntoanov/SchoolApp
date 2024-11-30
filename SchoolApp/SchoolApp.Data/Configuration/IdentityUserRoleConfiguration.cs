using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolApp.Data.Models;

namespace SchoolApp.Data.Configuration
{
    public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
        {
            builder.HasData(this.SeedUsersRoles());
        }

        private List<IdentityUserRole<Guid>> SeedUsersRoles()
        {
            List<IdentityUserRole<Guid>> usersRoles = new List<IdentityUserRole<Guid>>()
            {
                new IdentityUserRole<Guid>()
                {
                    UserId = Guid.Parse("1874d51f-29bc-4669-8f9d-938eaa55e4dd"),
                    RoleId = Guid.Parse("bc1bfaec-7297-48f0-a649-f290de46ad74")
                }
            };
            return usersRoles;
        }
    }
}