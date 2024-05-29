using Microsoft.EntityFrameworkCore;
using Task = Tribe.Domain.Models.Task.Task;

namespace Tribe.Domain.Database;

public interface IDataContext
{
    public DbSet<Models.Tribe.Tribe> Tribes { get; set; }
    public DbSet<Task> Tasks { get; set; }

    public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}