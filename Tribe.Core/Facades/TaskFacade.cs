using System.Net;
using Microsoft.AspNetCore.Identity;
using Tribe.Core.Mappers.DtoToModel;
using Tribe.Domain.Dto;
using Tribe.Domain.Facades;
using Tribe.Domain.Models.Task;
using Tribe.Domain.Models.User;
using Tribe.Domain.Repositories;
using Tribe.Domain.Services;
using TaskModel = Tribe.Domain.Models.Task.Task;
using TaskStatus = Tribe.Domain.Models.Task.TaskStatus;

namespace Tribe.Core.Facades;

public class TaskFacade(ITaskRepository taskRepository, ITribeRepository tribeRepository, UserManager<ApplicationUser> userManager, IUserService userService) : ITaskFacade
{
    public async Task<TaskDto> GetMyTaskAsync(Guid taskId, CancellationToken cancellationToken)
    {
        var task = (await GetByIdOrThrowAsync(taskId, cancellationToken)).ToDto();
        var userId = userService.GetUserIdOrThrow();
        
        if (task.CreatorId != userId && task.PerformerId != userId)
            throw new HttpRequestException(HttpRequestError.Unknown,
                statusCode: HttpStatusCode.NotFound, message: "Entity not found");

        return task;
    }

    public async Task<IReadOnlyCollection<TaskDto>> GetAllGivenTasksAsync(Guid tribeId, CancellationToken cancellationToken)
    {
        var userId = userService.GetUserIdOrThrow();

        var givenTasks = (await taskRepository.GetGivenAsync(userId, tribeId, cancellationToken)).Select(x => x.ToDto());

        return givenTasks.ToArray();
    }

    public async Task<IReadOnlyCollection<TaskDto>> GetAllTakenTasksAsync(Guid tribeId, CancellationToken cancellationToken)
    {
        var userId = userService.GetUserIdOrThrow();

        var takenTasks = (await taskRepository.GetTakenAsync(userId, tribeId, cancellationToken)).Select(x => x.ToDto());

        return takenTasks.ToArray();
    }

    public async Task<bool> GiveTaskAsync(TaskDto taskDto, CancellationToken cancellationToken)
    {
        var userId = userService.GetUserIdOrThrow();
        if (userId != taskDto.CreatorId)
            throw new HttpRequestException(HttpRequestError.Unknown,
                statusCode: HttpStatusCode.Forbidden, message: "Not enough rights");
        
        var tribe = await tribeRepository.GetByIdAsync(taskDto.TribeId, cancellationToken)
                    ?? throw new HttpRequestException(HttpRequestError.Unknown,
                statusCode: HttpStatusCode.NotFound, message: "Entity not found");

        var performerPosition = tribe.Positions.FirstOrDefault(x => x.UserId == taskDto.PerformerId) 
                                ?? throw new HttpRequestException(HttpRequestError.Unknown,
                                    statusCode: HttpStatusCode.NotFound, message: "Entity not found");
        
        if (performerPosition.ParentIds.All(x => x != userId))
            throw new HttpRequestException(HttpRequestError.Unknown,
                statusCode: HttpStatusCode.Forbidden, message: "Not enough rights");

        var creator = tribe.Participants.First(u => u.Id == taskDto.CreatorId);
        var performer = tribe.Participants.First(u => u.Id == taskDto.PerformerId);

        return await taskRepository.CreateAsync(taskDto.ToModel(tribe, creator, performer), cancellationToken);
    }

    public async Task<bool> ChangeNameAsync(Guid taskId, string newName, CancellationToken cancellationToken)
    {
        var creatorId = userService.GetUserIdOrThrow();
        var taskModel = await GetByIdOrThrowAsync(taskId, cancellationToken);
        var taskDto = taskModel.ToDto();
        
        if (!IsTaskCreator(taskDto, creatorId))
            throw new HttpRequestException(HttpRequestError.Unknown,
                statusCode: HttpStatusCode.Forbidden, message: "Not enough rights");

        taskModel.Name = newName;
        
        await taskRepository.UpdateAsync(taskModel, cancellationToken);

        return true;
    }

    public async Task<TaskDto> UpdateTaskContentAsync(Guid taskId, TaskContent taskContent, CancellationToken cancellationToken)
    {
        var creatorId = userService.GetUserIdOrThrow();
        var taskModel = await GetByIdOrThrowAsync(taskId, cancellationToken);
        
        if (!IsTaskCreator(taskModel.ToDto(), creatorId))
            throw new HttpRequestException(HttpRequestError.Unknown,
                statusCode: HttpStatusCode.Forbidden, message: "Not enough rights");

        taskModel.Content = taskContent;
        
        await taskRepository.UpdateAsync(taskModel, cancellationToken);

        return taskModel.ToDto();
    }
    
    public async Task<TaskDto> UpdateTaskStatusAsync(Guid taskId, TaskStatus taskStatus, CancellationToken cancellationToken)
    {
        var performerId = userService.GetUserIdOrThrow();
        var taskModel = await GetByIdOrThrowAsync(taskId, cancellationToken);
        
        if (!IsTaskPerformer(taskModel.ToDto(), performerId))
            throw new HttpRequestException(HttpRequestError.Unknown,
                statusCode: HttpStatusCode.Forbidden, message: "Not enough rights");

        taskModel.Status = taskStatus;
        
        await taskRepository.UpdateAsync(taskModel, cancellationToken);

        return taskModel.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid taskId, CancellationToken cancellationToken)
    {
        var creatorId = userService.GetUserIdOrThrow();
        var taskDto = (await GetByIdOrThrowAsync(taskId, cancellationToken)).ToDto();
        
        if (!IsTaskCreator(taskDto, creatorId))
            throw new HttpRequestException(HttpRequestError.Unknown,
                statusCode: HttpStatusCode.Forbidden, message: "Not enough rights");

        return await taskRepository.DeleteAsync(taskDto.Id, cancellationToken);
    }
    
    
    private async Task<TaskModel> GetByIdOrThrowAsync(Guid taskId, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetByIdAsync(taskId, cancellationToken) ??
                    throw new HttpRequestException(HttpRequestError.Unknown,
                        statusCode: HttpStatusCode.NotFound, message: "Entity not found");

        return task;
    }

    private bool IsTaskCreator(TaskDto taskDto, Guid userId) => taskDto.CreatorId == userId;
    
    private bool IsTaskPerformer(TaskDto taskDto, Guid userId) => taskDto.PerformerId == userId;
}