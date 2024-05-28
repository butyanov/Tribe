using Tribe.Domain.Models.Tribe;

namespace Tribe.Api.Contracts.Tribe.Requests;

public class TribeRequest
{
    public string Name { get; set; }
    
    public Guid CreatorId { get; set; }
    public IReadOnlyCollection<Guid> ParticipantsIds { get; set; }
    public IReadOnlyCollection<UserPosition> Positions { get; set; }
}