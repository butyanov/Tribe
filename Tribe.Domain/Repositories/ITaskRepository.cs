using Tribe.Domain.Dto;

namespace Tribe.Domain.Repositories;

public interface ITaskRepository
{
    public Task<bool> CreateAsync(TaskDto task, CancellationToken cancellationToken);

    public TaskDto UpdateAsync(TaskDto task, CancellationToken cancellationToken);

    public TaskDto GetByIdAsync(Guid taskId, CancellationToken cancellationToken);

    public IEnumerable<TaskDto> GetByUserAndTribeAsync(Guid userId, CancellationToken cancellationToken);

    public Task<bool> DeleteAsync(Guid taskId, CancellationToken cancellationToken);
}