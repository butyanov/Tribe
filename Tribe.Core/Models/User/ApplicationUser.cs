using Microsoft.AspNetCore.Identity;

namespace Tribe.Core.Models.User;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUser()
    {
        Id = Guid.NewGuid();
    }
   
}