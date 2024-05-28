using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tribe.Api.Contracts.Task.Requests;
using Tribe.Api.Contracts.Task.Responses;
using Tribe.Domain.Dto;
using Tribe.Domain.Facades;
using Tribe.Domain.Models.Task;

namespace Tribe.Api.Controllers;

// TODO: Написать валидаторы
[ApiController]
[Route($"tasks")]
public class TaskController(ITaskFacade taskFacade) : ControllerBase
{
    [Authorize]
    [HttpGet]
    [Route("get/{taskId:guid}")]    
    public async Task<TaskResponse> GetMyTask([FromRoute] Guid taskId, CancellationToken cancellationToken)
    {
        var task = await taskFacade.GetMyTaskAsync(taskId, cancellationToken);

        var tribeResponse = new TaskResponse()
        {
            Id = task.Id,
            CreatorId = task.CreatorId,
            TribeId = task.TribeId,
            Name = task.Name,
            Status = task.Status,
            Content = task.Content,
            PerformerId = task.PerformerId,
        };

        return tribeResponse;
    }
    
    [Authorize]
    [HttpGet]
    [Route("get-all-given/{tribeId:guid}")]    
    public async Task<IReadOnlyCollection<TaskResponse>> GetAllGivenTasksAsync([FromRoute] Guid tribeId, CancellationToken cancellationToken)
    {
        var tasks = await taskFacade.GetAllGivenTasksAsync(tribeId, cancellationToken);
        
        var tasksResponse = tasks.Select(task => new TaskResponse()
        {
            Id = task.Id,
            CreatorId = task.CreatorId,
            TribeId = task.TribeId,
            Name = task.Name,
            Status = task.Status,
            Content = task.Content,
            PerformerId = task.PerformerId,
        }).ToArray();
        
        return tasksResponse;
    }
    
    [Authorize]
    [HttpGet]
    [Route("get-all-taken/{tribeId:guid}")]    
    public async Task<IReadOnlyCollection<TaskResponse>> GetAllTakenTasksAsync([FromRoute] Guid tribeId, CancellationToken cancellationToken)
    {
        var tasks = await taskFacade.GetAllTakenTasksAsync(tribeId, cancellationToken);
        
        var tasksResponse = tasks.Select(task => new TaskResponse()
        {
            Id = task.Id,
            CreatorId = task.CreatorId,
            TribeId = task.TribeId,
            Name = task.Name,
            Status = task.Status,
            Content = task.Content,
            PerformerId = task.PerformerId,
        }).ToArray();
        
        return tasksResponse;
    }
    
    [Authorize]
    [HttpPost]
    [Route("create")]    
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var taskDto = new TaskDto()
        {
            Name = request.Name,
            Content = request.Content,
            TribeId = request.TribeId,
            CreatorId = request.CreatorId,
            PerformerId = request.PerformerId
        };
        
        await taskFacade.GiveTaskAsync(taskDto, cancellationToken);
        
        return Ok();
    }
    
    [Authorize]
    [HttpPost]
    [Route("change-name")]    
    public async Task<IActionResult> ChangeTaskName([FromBody] ChangeTaskNameRequest request, CancellationToken cancellationToken)
    {
        await taskFacade.ChangeNameAsync(request.TaskId, request.NewName, cancellationToken);
        return Ok();
    }
    
    [Authorize]
    [HttpPost]
    [Route("change-content/{taskId:guid}")]    
    public async Task<TaskResponse> ChangeTaskContent([FromRoute] Guid taskId, ChangeTaskContentRequest request, CancellationToken cancellationToken)
    {
        var taskContent = new TaskContent
        {
            Sections = request.Sections.Select(section => new TaskContent.Section
            {
                Label = section.Label,
                Input = new TaskContent.Input
                {
                    Content = section.Input.Content
                }
            })
        };
        
        var taskDto = await taskFacade.UpdateTaskContentAsync(taskId, taskContent, cancellationToken);

        var taskResponse = new TaskResponse
        {
            Id = taskDto.Id,
            Name = taskDto.Name,
            Content = taskDto.Content,
            Status = taskDto.Status,
            CreatorId = taskDto.CreatorId,
            TribeId = taskDto.TribeId,
            PerformerId = taskDto.TribeId
        };
        
        return taskResponse;
    }
    
    [Authorize]
    [HttpPost]
    [Route("change-status/{taskId:guid}")]    
    public async Task<TaskResponse> ChangeTaskStatus([FromRoute] Guid taskId, [FromBody] ChangeTaskStatusRequest request, CancellationToken cancellationToken)
    {
        var taskDto = await taskFacade.UpdateTaskStatusAsync(taskId, request.NewStatus, cancellationToken);
        
        var taskResponse = new TaskResponse
        {
            Id = taskDto.Id,
            Name = taskDto.Name,
            Content = taskDto.Content,
            Status = taskDto.Status,
            CreatorId = taskDto.CreatorId,
            TribeId = taskDto.TribeId,
            PerformerId = taskDto.TribeId
        };
        
        return taskResponse;
    }
    
    [Authorize]
    [HttpDelete]
    [Route("delete/{tribeId:guid}")]    
    public async Task<IActionResult> DeleteTribe([FromRoute] Guid taskId, CancellationToken cancellationToken)
    {
        await taskFacade.DeleteAsync(taskId, cancellationToken);
        return Ok();
    }
}