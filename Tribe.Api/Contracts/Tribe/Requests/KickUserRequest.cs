namespace Tribe.Api.Contracts.Tribe.Requests;

public class KickUserRequest
{
    public Guid TribeId { get; set; }
    public Guid UserId { get; set; }
}