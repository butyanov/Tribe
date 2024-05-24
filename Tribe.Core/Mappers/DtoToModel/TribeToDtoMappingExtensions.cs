using Tribe.Domain.Dto;
using TribeModel = Tribe.Domain.Models.Tribe.Tribe;

namespace Tribe.Core.Mappers.DtoToModel;

public static class TribeToDtoMappingExtensions
{
    public static TribeDto ToDto(this TribeModel tribe)
    {
        return new TribeDto
        {
            Id = tribe.Id,
            Name = tribe.Name,
            Participants = tribe.Participants,
            Positions = tribe.Positions
        };
    }
    
    public static TribeModel ToModel(this TribeDto tribeDto)
    {
        return new TribeModel
        {
            Name = tribeDto.Name,
            Participants = tribeDto.Participants,
            Positions = tribeDto.Positions
        };
    }
}