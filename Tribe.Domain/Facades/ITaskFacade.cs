using Tribe.Domain.Dto;
using Tribe.Domain.Models.Task;
using TaskStatus = Tribe.Domain.Models.Task.TaskStatus;

namespace Tribe.Domain.Facades;

public interface ITaskFacade
{
    public Task<TaskDto> GetMyTaskAsync(Guid taskId, CancellationToken cancellationToken);
    public Task<IReadOnlyCollection<TaskDto>> GetAllGivenTasksAsync(Guid tribeId, CancellationToken cancellationToken);
    public Task<IReadOnlyCollection<TaskDto>> GetAllTakenTasksAsync(Guid tribeId, CancellationToken cancellationToken);

    public Task<bool> GiveTaskAsync(TaskDto taskDto, CancellationToken cancellationToken);
    public Task<bool> ChangeNameAsync(Guid taskId, string newName, CancellationToken cancellationToken);

    public Task<TaskDto> UpdateTaskContentAsync(Guid taskId, TaskContent taskContent,
        CancellationToken cancellationToken);

    public Task<TaskDto> UpdateTaskStatusAsync(Guid taskId, TaskStatus taskStatus, CancellationToken cancellationToken);
    public Task<bool> DeleteAsync(Guid taskId, CancellationToken cancellationToken);
}