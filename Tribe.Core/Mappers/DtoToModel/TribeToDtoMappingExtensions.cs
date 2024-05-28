using Tribe.Domain.Dto;
using Tribe.Domain.Models.User;
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
            CreatorId = tribe.Creator.Id,
            ParticipantsIds = tribe.Participants.Select(x => x.Id).ToArray(),
            Positions = tribe.Positions.ToArray()
        };
    }

    public static TribeModel ToModel(this TribeDto tribeDto, ApplicationUser creator,
        IEnumerable<ApplicationUser> participants)

    {
        return new TribeModel
        {
            Name = tribeDto.Name,
            Creator = creator,
            Participants = participants.ToArray(),
            Positions = tribeDto.Positions,
            CreatorId = creator.Id
        };
    }
}