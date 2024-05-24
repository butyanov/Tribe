using Tribe.Domain.Models.User;

namespace Tribe.Domain.Models.Task;

public class Task : BaseEntity
{
    public string Name { get; set; }
    public TaskContent Content { get; set; }
    public ApplicationUser Creator { get; set; }
    public ApplicationUser Performer { get; set; }
}