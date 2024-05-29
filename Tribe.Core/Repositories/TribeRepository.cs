using Microsoft.EntityFrameworkCore;
using Tribe.Domain.Database;
using Tribe.Domain.Repositories;
using TribeModel = Tribe.Domain.Models.Tribe.Tribe;

namespace Tribe.Core.Repositories;

public class TribeRepository(IDataContext dataContext) : ITribeRepository
{
    public async Task<bool> CreateAsync(TribeModel tribe, CancellationToken cancellationToken)
    {
        await dataContext.Tribes.AddAsync(tribe, cancellationToken);

        return await dataContext.SaveEntitiesAsync(cancellationToken);
    }

    public async Task<TribeModel?> UpdateAsync(TribeModel tribe, CancellationToken cancellationToken)
    {
        var currentTribe = await dataContext.Tribes.FirstOrDefaultAsync(x => x.Id == tribe.Id, cancellationToken);
        if (currentTribe == null)
            return default;

        currentTribe.Name = tribe.Name;
        currentTribe.Positions = tribe.Positions;
        currentTribe.Participants = tribe.Participants;

        await dataContext.SaveEntitiesAsync(cancellationToken);

        return currentTribe;
    }

    public async Task<TribeModel?> GetByIdAsync(Guid tribeId, CancellationToken cancellationToken)
    {
        return await dataContext.Tribes.Include(t => t.Participants)
            .FirstOrDefaultAsync(x => x.Id == tribeId, cancellationToken);
    }

    public async Task<IEnumerable<TribeModel>> GetByUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await dataContext.Tribes.Include(t => t.Participants)
            .Where(t => t.Participants.Any(p => p.Id == userId))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid tribeId, CancellationToken cancellationToken)
    {
        var tribeToDelete = await dataContext.Tribes.FirstOrDefaultAsync(x => x.Id == tribeId, cancellationToken);
        if (tribeToDelete == default)
            return false;

        dataContext.Tribes.Remove(tribeToDelete);

        return await dataContext.SaveEntitiesAsync(cancellationToken);
    }
}