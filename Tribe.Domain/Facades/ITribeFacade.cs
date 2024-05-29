using Tribe.Domain.Dto;

namespace Tribe.Domain.Facades;

public interface ITribeFacade
{
    public Task<TribeDto> GetMyTribeAsync(Guid tribeId, CancellationToken cancellationToken);
    public Task<IReadOnlyCollection<TribeDto>> GetAllMyTribesAsync(CancellationToken cancellationToken);

    public Task<bool> CreateAsync(TribeDto tribeDto, CancellationToken cancellationToken);

    public Task<bool> AddUserAsync(Guid tribeId, Guid userId, IReadOnlyCollection<Guid> leads,
        IReadOnlyCollection<Guid> subordinates, CancellationToken cancellationToken);

    public Task<bool> KickUserAsync(Guid tribeId, Guid userId, CancellationToken cancellationToken);
    public Task<bool> ChangeNameAsync(Guid tribeId, string newName, CancellationToken cancellationToken);
    public Task<bool> DeleteAsync(Guid tribeId, CancellationToken cancellationToken);
}