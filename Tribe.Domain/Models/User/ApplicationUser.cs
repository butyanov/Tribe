using Microsoft.AspNetCore.Identity;

namespace Tribe.Domain.Models.User;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public ICollection<Tribe.Tribe> Tribes { get; set; }
    public ApplicationUser()
    {
        Id = Guid.NewGuid();
    }
   
}