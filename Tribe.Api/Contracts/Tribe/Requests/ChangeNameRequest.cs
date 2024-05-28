namespace Tribe.Api.Contracts.Tribe.Requests;

public class ChangeNameRequest
{
    public Guid TribeId { get; set; }
    public string NewName { get; set; }
}