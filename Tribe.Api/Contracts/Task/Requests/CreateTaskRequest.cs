using Tribe.Domain.Models.Task;

namespace Tribe.Api.Contracts.Task.Requests;

public class CreateTaskRequest
{
    public string Name { get; set; }
    public TaskContent Content { get; set; }
    public Guid TribeId { get; set; }
    public Guid PerformerId { get; set; }
}