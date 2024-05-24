using Tribe.Domain.Dto;
using Task = Tribe.Domain.Models.Task.Task;

namespace Tribe.Core.Mappers.DtoToModel;

public static class TaskToDtoMappingExtensions
{
    public static TaskDto ToDto(this Task tribe)
    {
        return new TaskDto
        {
            Id = tribe.Id,
            Content = tribe.Content,
            Name = tribe.Name,
            Creator = tribe.Creator,
            Performer = tribe.Performer
        };
    }
    
    public static Task ToModel(this TaskDto tribeDto)
    {
        return new Task
        {
            Content = tribeDto.Content,
            Name = tribeDto.Name,
            Creator = tribeDto.Creator,
            Performer = tribeDto.Performer
        };
    }
}