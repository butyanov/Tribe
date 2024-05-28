using Tribe.Domain.Models.Tribe;

namespace Tribe.Api.Contracts.Tribe.Responses;

public class TribeResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public Guid CreatorId { get; set; }
    public IReadOnlyCollection<Guid> ParticipantsIds { get; set; }
    public IReadOnlyCollection<UserPosition> Positions { get; set; }
}