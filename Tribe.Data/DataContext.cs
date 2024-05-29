using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tribe.Domain.Database;
using Tribe.Domain.Database.Abstractions;
using Tribe.Domain.Models.User;
using TaskModel = Tribe.Domain.Models.Task.Task;
using TribeModel = Tribe.Domain.Models.Tribe.Tribe;

namespace Tribe.Data;

public class DataContext(
    DbContextOptions<DataContext> options,
    IEnumerable<DependencyInjectedEntityConfiguration> configurations)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options), IDataContext
{
    public DbSet<TribeModel> Tribes { get; set; }
    public DbSet<TaskModel> Tasks { get; set; }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        return result > 0;
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var config in configurations)
            config.Configure(builder);
    }
}