namespace Tribe.Domain.Models;

public class BaseEntity
{
    public Guid Id { get; protected init; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }
}