using TribeModel = Tribe.Domain.Models.Tribe.Tribe;

namespace Tribe.Domain.Repositories;

public interface ITribeRepository
{
    public Task<bool> CreateAsync(TribeModel tribe, CancellationToken cancellationToken);

    public Task<TribeModel?> UpdateAsync(TribeModel tribe, CancellationToken cancellationToken);

    public Task<TribeModel?> GetByIdAsync(Guid tribeId, CancellationToken cancellationToken);

    public Task<IEnumerable<TribeModel>> GetByUserAsync(Guid userId, CancellationToken cancellationToken);

    public Task<bool> DeleteAsync(Guid tribeId, CancellationToken cancellationToken);
}