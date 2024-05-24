using Tribe.Domain.Models.Tribe;
using Tribe.Domain.Models.User;

namespace Tribe.Domain.Dto;

public class TribeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<ApplicationUser> Participants { get; set; }
    public IEnumerable<UserPosition> Positions { get; set; }
}