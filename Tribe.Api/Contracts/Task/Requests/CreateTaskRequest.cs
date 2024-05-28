using Tribe.Domain.Models.Task;
using TaskStatus = Tribe.Domain.Models.Task.TaskStatus;

namespace Tribe.Api.Contracts.Task.Requests;

public class CreateTaskRequest
{
    public string Name { get; set; }
    public TaskStatus Status { get; set; }
    public TaskContent Content { get; set; }
    public Guid TribeId { get; set; }
    public Guid CreatorId { get; set; }
    public Guid PerformerId { get; set; }
}