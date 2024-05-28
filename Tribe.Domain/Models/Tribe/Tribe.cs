using Tribe.Domain.Models.User;

namespace Tribe.Domain.Models.Tribe;

public class Tribe : BaseEntity
{
    public required string Name { get; set; }
    public required Guid CreatorId { get; set; }
    public required ApplicationUser Creator { get; set; }
    public required ICollection<ApplicationUser> Participants { get; set; }
    public required IEnumerable<UserPosition> Positions { get; set; }
}