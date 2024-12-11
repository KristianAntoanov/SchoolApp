using Microsoft.AspNetCore.Identity;

namespace SchoolApp.Data.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUser()
    {
        this.Id = Guid.NewGuid();
    }
}