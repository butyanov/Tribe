using Tribe.Core.Mappers.DtoToModel;
using Tribe.Domain.Database;
using Tribe.Domain.Dto;
using Tribe.Domain.Repositories;

namespace Tribe.Core.Repositories;

public class TaskRepository(IDataContext dataContext) : ITaskRepository
{
    public async Task<bool> CreateAsync(TaskDto task, CancellationToken cancellationToken)
    {
        var taskModel = task.ToModel();

        await dataContext.Tasks.AddAsync(taskModel, cancellationToken);

        return await dataContext.SaveEntitiesAsync(cancellationToken);
    }

    public TaskDto UpdateAsync(TaskDto task, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public TaskDto GetByIdAsync(Guid taskId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TaskDto> GetByUserAndTribeAsync(Guid userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid taskId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}