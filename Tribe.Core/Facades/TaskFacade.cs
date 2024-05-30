using Microsoft.AspNetCore.Identity;
using Tribe.Core.ClientExceptions;
using Tribe.Core.Mappers.DtoToModel;
using Tribe.Domain.Dto;
using Tribe.Domain.Facades;
using Tribe.Domain.Models.Task;
using Tribe.Domain.Models.Tribe;
using Tribe.Domain.Models.User;
using Tribe.Domain.Repositories;
using Tribe.Domain.Services;
using TaskModel = Tribe.Domain.Models.Task.Task;
using TaskStatus = Tribe.Domain.Models.Task.TaskStatus;

namespace Tribe.Core.Facades;

public class TaskFacade(ITaskRepository taskRepository, ITribeRepository tribeRepository, IUserService userService, UserManager<ApplicationUser> userManager)
    : ITaskFacade
{
    public async Task<TaskDto> GetMyTaskAsync(Guid taskId, CancellationToken cancellationToken)
    {
        var task = (await GetByIdOrThrowAsync(taskId, cancellationToken)).ToDto();
        var userId = userService.GetUserIdOrThrow();

        if (task.CreatorId != userId && task.PerformerId != userId)
            throw new NotFoundException<TaskModel>();

        return task;
    }

    public async Task<IReadOnlyCollection<TaskDto>> GetAllGivenTasksAsync(Guid tribeId,
        CancellationToken cancellationToken)
    {
        var userId = userService.GetUserIdOrThrow();

        var givenTasks =
            (await taskRepository.GetGivenAsync(userId, tribeId, cancellationToken)).Select(x => x.ToDto());

        return givenTasks.ToArray();
    }

    public async Task<IReadOnlyCollection<TaskDto>> GetAllTakenTasksAsync(Guid tribeId,
        CancellationToken cancellationToken)
    {
        var userId = userService.GetUserIdOrThrow();

        var takenTasks =
            (await taskRepository.GetTakenAsync(userId, tribeId, cancellationToken)).Select(x => x.ToDto());

        return takenTasks.ToArray();
    }
    
    public async Task<IReadOnlyCollection<TaskDto>> GetAllGivenTasksAsync(Guid tribeId, Guid userId,
        CancellationToken cancellationToken)
    {
        var user = userManager.Users.FirstOrDefault(u => u.Id == userId) ??
                   throw new NotFoundException<ApplicationUser>();
        
        var givenTasks =
            (await taskRepository.GetGivenAsync(user.Id, tribeId, cancellationToken)).Select(x => x.ToDto());

        return givenTasks.ToArray();
    }

    public async Task<IReadOnlyCollection<TaskDto>> GetAllTakenTasksAsync(Guid tribeId, Guid userId,
        CancellationToken cancellationToken)
    {
        var user = userManager.Users.FirstOrDefault(u => u.Id == userId) ??
                   throw new NotFoundException<ApplicationUser>();
        
        var takenTasks =
            (await taskRepository.GetTakenAsync(user.Id, tribeId, cancellationToken)).Select(x => x.ToDto());

        return takenTasks.ToArray();
    }

    public async Task<bool> GiveTaskAsync(TaskDto taskDto, CancellationToken cancellationToken)
    {
        var userId = userService.GetUserIdOrThrow();
        taskDto.CreatorId = userId;

        var tribe = await tribeRepository.GetByIdAsync(taskDto.TribeId, cancellationToken)
                    ?? throw new NotFoundException<TaskModel>();

        var performerPosition = tribe.Positions.FirstOrDefault(x => x.UserId == taskDto.PerformerId)
                                ?? throw new NotFoundException<UserPosition>();

        if (performerPosition.ParentIds.All(x => x != userId))
            throw new ForbiddenException("FORBIDDEN");

        var creator = tribe.Participants.First(u => u.Id == taskDto.CreatorId);
        var performer = tribe.Participants.First(u => u.Id == taskDto.PerformerId);

        return await taskRepository.CreateAsync(taskDto.ToModel(tribe, creator, performer), cancellationToken);
    }

    public async Task<bool> ChangeNameAsync(Guid taskId, string newName, CancellationToken cancellationToken)
    {
        var creatorId = userService.GetUserIdOrThrow();
        var taskModel = await GetByIdOrThrowAsync(taskId, cancellationToken);

        ValidateTaskCreatorRights(taskModel.ToDto(), creatorId);

        taskModel.Name = newName;

        await taskRepository.UpdateAsync(taskModel, cancellationToken);

        return true;
    }

    public async Task<TaskDto> UpdateTaskContentAsync(Guid taskId, TaskContent taskContent,
        CancellationToken cancellationToken)
    {
        var creatorId = userService.GetUserIdOrThrow();
        var taskModel = await GetByIdOrThrowAsync(taskId, cancellationToken);

        ValidateTaskCreatorRights(taskModel.ToDto(), creatorId);

        taskModel.Content = taskContent;

        await taskRepository.UpdateAsync(taskModel, cancellationToken);

        return taskModel.ToDto();
    }

    public async Task<TaskDto> UpdateTaskStatusAsync(Guid taskId, TaskStatus taskStatus,
        CancellationToken cancellationToken)
    {
        var performerId = userService.GetUserIdOrThrow();
        var taskModel = await GetByIdOrThrowAsync(taskId, cancellationToken);

        ValidateTaskPerformerRights(taskModel.ToDto(), performerId);

        taskModel.Status = taskStatus;

        await taskRepository.UpdateAsync(taskModel, cancellationToken);

        return taskModel.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid taskId, CancellationToken cancellationToken)
    {
        var creatorId = userService.GetUserIdOrThrow();
        var taskDto = (await GetByIdOrThrowAsync(taskId, cancellationToken)).ToDto();

        ValidateTaskCreatorRights(taskDto, creatorId);

        return await taskRepository.DeleteAsync(taskDto.Id, cancellationToken);
    }


    private async Task<TaskModel> GetByIdOrThrowAsync(Guid taskId, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetByIdAsync(taskId, cancellationToken) ??
                   throw new NotFoundException<TaskModel>();

        return task;
    }

    private static void ValidateTaskCreatorRights(TaskDto task, Guid userId)
    {
        if (task.CreatorId != userId)
            throw new ForbiddenException("FORBIDDEN");
    }

    private static void ValidateTaskPerformerRights(TaskDto task, Guid userId)
    {
        if (task.PerformerId != userId)
            throw new ForbiddenException("FORBIDDEN");
    }
}