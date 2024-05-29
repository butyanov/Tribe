using Microsoft.EntityFrameworkCore;
using Tribe.Domain.Database;
using Tribe.Domain.Repositories;
using TaskModel = Tribe.Domain.Models.Task.Task;

namespace Tribe.Core.Repositories;

public class TaskRepository(IDataContext dataContext) : ITaskRepository
{
    public async Task<bool> CreateAsync(TaskModel task, CancellationToken cancellationToken)
    {
        await dataContext.Tasks.AddAsync(task, cancellationToken);

        return await dataContext.SaveEntitiesAsync(cancellationToken);
    }

    public async Task<TaskModel?> UpdateAsync(TaskModel task, CancellationToken cancellationToken)
    {
        var currentTask = await dataContext.Tasks.FirstOrDefaultAsync(x => x.Id == task.Id, cancellationToken);
        if (currentTask == null)
            return default;

        currentTask.Name = task.Name;
        currentTask.Content = task.Content;

        await dataContext.SaveEntitiesAsync(cancellationToken);

        return currentTask;
    }

    public async Task<TaskModel?> GetByIdAsync(Guid taskId, CancellationToken cancellationToken)
    {
        return await dataContext.Tasks
            .Include(x => x.Creator)
            .Include(x => x.Tribe)
            .Include(x => x.Performer)
            .FirstOrDefaultAsync(x => x.Id == taskId, cancellationToken);
    }

    public async Task<IEnumerable<TaskModel>> GetGivenAsync(Guid userId, Guid tribeId,
        CancellationToken cancellationToken)
    {
        return await dataContext.Tasks
            .Include(x => x.Creator)
            .Include(x => x.Tribe)
            .Include(x => x.Performer)
            .Where(x => x.Creator.Id == userId && x.Tribe.Id == tribeId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TaskModel>> GetTakenAsync(Guid userId, Guid tribeId,
        CancellationToken cancellationToken)
    {
        return await dataContext.Tasks
            .Include(x => x.Creator)
            .Include(x => x.Tribe)
            .Include(x => x.Performer)
            .Where(x => x.Performer.Id == userId && x.Tribe.Id == tribeId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid taskId, CancellationToken cancellationToken)
    {
        var taskToDelete = await dataContext.Tasks.FirstOrDefaultAsync(x => x.Id == taskId, cancellationToken);
        if (taskToDelete == default)
            return false;

        dataContext.Tasks.Remove(taskToDelete);

        return await dataContext.SaveEntitiesAsync(cancellationToken);
    }
}