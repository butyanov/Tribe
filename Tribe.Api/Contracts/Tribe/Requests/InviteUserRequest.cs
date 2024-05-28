namespace Tribe.Api.Contracts.Tribe.Requests;

public class InviteUserRequest
{
    public Guid TribeId { get; set; }
    public Guid UserId { get; set; }
    public IReadOnlyCollection<Guid> Leads { get; set; }
    public IReadOnlyCollection<Guid> Subordinates { get; set; }
}