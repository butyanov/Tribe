using TaskStatus = Tribe.Domain.Models.Task.TaskStatus;

namespace Tribe.Api.Contracts.Task.Requests;

public class ChangeTaskStatusRequest
{
    public TaskStatus NewStatus { get; set; }
}