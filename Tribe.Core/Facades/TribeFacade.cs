using Microsoft.AspNetCore.Identity;
using Tribe.Core.ClientExceptions;
using Tribe.Core.Mappers.DtoToModel;
using Tribe.Domain.Dto;
using Tribe.Domain.Facades;
using Tribe.Domain.Models.Tribe;
using Tribe.Domain.Models.User;
using Tribe.Domain.Repositories;
using Tribe.Domain.Services;
using TribeModel = Tribe.Domain.Models.Tribe.Tribe;

namespace Tribe.Core.Facades;

public class TribeFacade(
    ITribeRepository tribeRepository,
    UserManager<ApplicationUser> userManager,
    IUserService userService) : ITribeFacade
{
    public async Task<TribeDto> GetMyTribeAsync(Guid tribeId, CancellationToken cancellationToken)
    {
        var userId = userService.GetUserIdOrThrow();
        var tribes = (await tribeRepository.GetByUserAsync(userId, cancellationToken)).ToArray();

        var wantedTribe = tribes.FirstOrDefault(x => x.Id == tribeId)
                          ?? throw new NotFoundException<TribeModel>();

        return wantedTribe.ToDto();
    }

    public async Task<IReadOnlyCollection<TribeDto>> GetAllMyTribesAsync(CancellationToken cancellationToken)
    {
        var userId = userService.GetUserIdOrThrow();
        return (await tribeRepository.GetByUserAsync(userId, cancellationToken)).Select(x => x.ToDto()).ToArray();
    }

    public async Task<bool> CreateAsync(TribeDto tribeDto, CancellationToken cancellationToken)
    {
        var currentUser = userManager.Users.FirstOrDefault(x => x.Id == userService.GetUserIdOrThrow())
                          ?? throw new NotFoundException<ApplicationUser>();

        var tribeMembers = new List<ApplicationUser>();

        tribeDto.CreatorId = currentUser.Id;
        tribeMembers.Add(currentUser);

        foreach (var user in userManager.Users)
            if (tribeDto.ParticipantsIds.Contains(user.Id))
                tribeMembers.Add(user);

        var tribeModel = tribeDto.ToModel(currentUser, tribeMembers.ToHashSet());

        if (tribeModel.Participants.Count != tribeModel.Positions.Count())
            throw new ClientException("Inconsistent hiearchy");

        if ((await tribeRepository.GetByUserAsync(currentUser.Id, cancellationToken)).Any(x => x.Name == tribeDto.Name))
            throw new AlreadyExistsException("Task");

        return await tribeRepository.CreateAsync(tribeModel, cancellationToken);
    }

    public async Task<bool> AddUserAsync(Guid tribeId, Guid userId, IReadOnlyCollection<Guid> leads,
        IReadOnlyCollection<Guid> subordinates, CancellationToken cancellationToken)
    {
        var ownerId = userService.GetUserIdOrThrow();
        var tribeModel = await GetByIdOrThrowAsync(tribeId, cancellationToken);

        ValidateTribeRights(tribeModel, ownerId);

        if (tribeModel.Participants.Select(p => p.Id).Contains(userId))
            throw new ClientException("User already invited");

        foreach (var member in leads.Concat(subordinates).ToArray())
        {
            var tribeMembers = tribeModel.Participants.ToArray();
            if (!tribeMembers.Select(x => x.Id).Contains(member))
                throw new ClientException($"{member} is not a member of tribe");
        }

        var appUser = userManager.Users.FirstOrDefault(x => x.Id == userId) ??
                      throw new NotFoundException<ApplicationUser>();

        tribeModel.Participants = tribeModel.Participants.Append(appUser).ToArray();
        tribeModel.Positions = tribeModel.Positions.Append(new UserPosition
        {
            UserId = appUser.Id,
            ChildrenIds = subordinates,
            ParentIds = leads
        }).ToArray();

        await tribeRepository.UpdateAsync(tribeModel, cancellationToken);

        return true;
    }

    public async Task<bool> KickUserAsync(Guid tribeId, Guid userId,
        CancellationToken cancellationToken)
    {
        var ownerId = userService.GetUserIdOrThrow();
        var tribeModel = await GetByIdOrThrowAsync(tribeId, cancellationToken);

        ValidateTribeRights(tribeModel, ownerId);

        if (ownerId == userId)
            throw new ClientException("You cannot kick yourself from your own tribe");

        var appUser = userManager.Users.FirstOrDefault(x => x.Id == userId) ??
                      throw new NotFoundException<ApplicationUser>();

        tribeModel.Participants.Remove(appUser);
        tribeModel.Positions = tribeModel.Positions.Where(x => x.UserId != appUser.Id).ToArray();

        await tribeRepository.UpdateAsync(tribeModel, cancellationToken);

        return true;
    }

    public async Task<bool> ChangeNameAsync(Guid tribeId, string newName, CancellationToken cancellationToken)
    {
        var ownerId = userService.GetUserIdOrThrow();
        var tribeModel = await GetByIdOrThrowAsync(tribeId, cancellationToken);

        ValidateTribeRights(tribeModel, ownerId);

        tribeModel.Name = newName;

        await tribeRepository.UpdateAsync(tribeModel, cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid tribeId, CancellationToken cancellationToken)
    {
        var ownerId = userService.GetUserIdOrThrow();
        var tribeModel = await GetByIdOrThrowAsync(tribeId, cancellationToken);

        ValidateTribeRights(tribeModel, ownerId);

        return await tribeRepository.DeleteAsync(tribeId, cancellationToken);
    }

    private async Task<TribeModel> GetByIdOrThrowAsync(Guid tribeId, CancellationToken cancellationToken)
    {
        return await tribeRepository.GetByIdAsync(tribeId, cancellationToken) ??
               throw new NotFoundException<TribeModel>();
    }

    private static void ValidateTribeRights(TribeModel tribe, Guid userId)
    {
        if (tribe.CreatorId != userId)
            throw new ForbiddenException("FORBIDDEN");
    }
}