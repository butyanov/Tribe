using Tribe.Domain.Models.User;

namespace Tribe.Domain.Models.Task;

public class Task : BaseEntity
{
    public required string Name { get; set; }

    public required TaskStatus Status { get; set; } = TaskStatus.Created;
    public required TaskContent Content { get; set; }
    public required Tribe.Tribe Tribe { get; set; }
    public required ApplicationUser Creator { get; set; }
    public required ApplicationUser Performer { get; set; }
}