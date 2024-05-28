namespace Tribe.Api.Contracts.Task.Requests;

public class ChangeTaskNameRequest
{
    public Guid TaskId { get; set; }
    public string NewName { get; set; }
}