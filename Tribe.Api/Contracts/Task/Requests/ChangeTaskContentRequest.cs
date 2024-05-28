using Tribe.Domain.Models.Task;

namespace Tribe.Api.Contracts.Task.Requests;

public class ChangeTaskContentRequest
{
    public IEnumerable<TaskContent.Section> Sections { get; set; }
}