using Tribe.Domain.Models.Task;
using Tribe.Domain.Models.User;

namespace Tribe.Domain.Dto;

public class TaskDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public TaskContent Content { get; set; }
    public ApplicationUser Creator { get; set; }
    public ApplicationUser Performer { get; set; }
}