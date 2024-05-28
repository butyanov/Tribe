using Tribe.Domain.Models.Tribe;

namespace Tribe.Domain.Dto;

public class TribeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public Guid CreatorId { get; set; }
    public IReadOnlyCollection<Guid> ParticipantsIds { get; set; }
    public IReadOnlyCollection<UserPosition> Positions { get; set; }
}