using Tribe.Domain.Models.Task;
using TaskStatus = Tribe.Domain.Models.Task.TaskStatus;

namespace Tribe.Api.Contracts.Task.Responses;

public class TaskResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public required TaskStatus Status { get; set; } = TaskStatus.Created;
    public TaskContent Content { get; set; }
    public Guid TribeId { get; set; }
    public Guid CreatorId { get; set; }
    public Guid PerformerId { get; set; }
}