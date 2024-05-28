using TaskModel = Tribe.Domain.Models.Task.Task;

namespace Tribe.Domain.Repositories;

public interface ITaskRepository
{
    public Task<bool> CreateAsync(TaskModel task, CancellationToken cancellationToken);

    public Task<TaskModel?> UpdateAsync(TaskModel task, CancellationToken cancellationToken);

    public Task<TaskModel?> GetByIdAsync(Guid taskId, CancellationToken cancellationToken);

    public Task<IEnumerable<TaskModel>> GetGivenAsync(Guid userId, Guid tribeId, CancellationToken cancellationToken);

    public Task<IEnumerable<TaskModel>> GetTakenAsync(Guid userId, Guid tribeId, CancellationToken cancellationToken);

    public Task<bool> DeleteAsync(Guid taskId, CancellationToken cancellationToken);
}