using Tribe.Domain.Dto;
using TribeModel = Tribe.Domain.Models.Tribe.Tribe;

namespace Tribe.Domain.Repositories;

public interface ITribeRepository
{
    public Task<bool> CreateAsync(TribeDto tribe);

    public TribeDto UpdateAsync(TribeDto tribe);

    public TribeDto GetByIdAsync(Guid tribeId);

    public IEnumerable<TribeDto> GetByUserAsync(Guid userId);

    public Task<bool> DeleteAsync(Guid tribeId);
}