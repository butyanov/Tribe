namespace Tribe.Domain.Models.Tribe;

public class UserPosition
{
    public Guid UserId { get; set; }
    public IEnumerable<Guid>? ParentIds { get; set; }
    public IEnumerable<Guid>?  ChildrenIds { get; set; }
}