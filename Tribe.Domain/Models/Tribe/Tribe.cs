using Tribe.Domain.Models.User;

namespace Tribe.Domain.Models.Tribe;

public class Tribe : BaseEntity
{
    public string Name { get; set; }
    public IEnumerable<ApplicationUser> Participants { get; set; }
    public IEnumerable<UserPosition> Positions { get; set; }
}