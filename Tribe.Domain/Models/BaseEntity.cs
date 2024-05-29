namespace Tribe.Domain.Models;

public class BaseEntity
{
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; protected init; }
}