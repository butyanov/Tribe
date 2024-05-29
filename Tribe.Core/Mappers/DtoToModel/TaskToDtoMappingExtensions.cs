using Tribe.Domain.Dto;
using Tribe.Domain.Models.User;
using Task = Tribe.Domain.Models.Task.Task;

namespace Tribe.Core.Mappers.DtoToModel;

public static class TaskToDtoMappingExtensions
{
    public static TaskDto ToDto(this Task task)
    {
        return new TaskDto
        {
            Id = task.Id,
            Name = task.Name,
            Status = task.Status,
            Content = task.Content,
            TribeId = task.Tribe.Id,
            CreatorId = task.Creator.Id,
            PerformerId = task.Performer.Id
        };
    }

    public static Task ToModel(this TaskDto taskDto,
        Domain.Models.Tribe.Tribe tribe,
        ApplicationUser creator,
        ApplicationUser performer)
    {
        return new Task
        {
            Name = taskDto.Name,
            Status = taskDto.Status,
            Content = taskDto.Content,
            Tribe = tribe,
            Creator = creator,
            Performer = performer
        };
    }
}