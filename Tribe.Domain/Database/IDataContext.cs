using Microsoft.EntityFrameworkCore;

namespace Tribe.Domain.Database;

public interface IDataContext
{
    public DbSet<Domain.Models.Tribe.Tribe> Tribes { get; set; }
    public DbSet<Domain.Models.Task.Task> Tasks { get; set; }

    public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}