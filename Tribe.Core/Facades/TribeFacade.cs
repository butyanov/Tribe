using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Tribe.Core.Mappers.DtoToModel;
using Tribe.Domain.Dto;
using Tribe.Domain.Facades;
using Tribe.Domain.Models.Tribe;
using Tribe.Domain.Models.User;
using Tribe.Domain.Repositories;
using Tribe.Domain.Services;
using TribeModel = Tribe.Domain.Models.Tribe.Tribe;

namespace Tribe.Core.Facades;

public class TribeFacade(ITribeRepository tribeRepository, UserManager<ApplicationUser> userManager, IUserService userService) : ITribeFacade
{
    public async Task<TribeDto> GetMyTribeAsync(Guid tribeId, CancellationToken cancellationToken)
    {
        var userId = userService.GetUserIdOrThrow();
        var tribes = (await tribeRepository.GetByUserAsync(userId, cancellationToken)).ToArray();

        var wantedTribe = tribes.FirstOrDefault(x => x.Id == tribeId)
                          ?? throw new HttpRequestException(HttpRequestError.Unknown,
                              statusCode: HttpStatusCode.NotFound, message: "Entity not found");

        return wantedTribe.ToDto();
    }

    public async Task<IReadOnlyCollection<TribeDto>> GetAllMyTribesAsync(CancellationToken cancellationToken)
    {
        var userId = userService.GetUserIdOrThrow();
        return (await tribeRepository.GetByUserAsync(userId, cancellationToken)).Select(x => x.ToDto()).ToArray();
    }

    public async Task<bool> CreateAsync(TribeDto tribeDto, CancellationToken cancellationToken)
    {
        var currentUser = userManager.Users.FirstOrDefault(x => x.Id == userService.GetUserIdOrThrow()) ??
                          throw new HttpRequestException(HttpRequestError.Unknown,
                              statusCode: HttpStatusCode.NotFound, message: "Entity not found");
        var tribeMembers = new List<ApplicationUser>();

        tribeDto.CreatorId = currentUser.Id;
        tribeMembers.Add(currentUser);
        
        foreach (var user in userManager.Users)
        {
            if (tribeDto.ParticipantsIds.Contains(user.Id))
                tribeMembers.Add(user);
        }

        var tribeModel = tribeDto.ToModel(currentUser, tribeMembers.ToHashSet());
        
        if (tribeModel.Participants.Count != tribeModel.Positions.Count())
            throw new HttpRequestException(HttpRequestError.Unknown,
                statusCode: HttpStatusCode.BadRequest, message: "Inconsistent hiearchy");
        
        var alreadyExists =
            (await tribeRepository.GetByUserAsync(currentUser.Id, cancellationToken)).Any(x => x.Name == tribeDto.Name);
        if (alreadyExists) 
            throw new HttpRequestException(HttpRequestError.Unknown,
                                  statusCode: HttpStatusCode.BadRequest, message: "Entity already exists");
        
        return await tribeRepository.CreateAsync(tribeModel, cancellationToken);
    }

    public async Task<bool> AddUserAsync(Guid tribeId, Guid userId, IReadOnlyCollection<Guid> leads,
        IReadOnlyCollection<Guid> subordinates, CancellationToken cancellationToken)
    {
        var ownerId = userService.GetUserIdOrThrow();
        var tribeModel = await GetByIdOrThrowAsync(tribeId, cancellationToken);
        
        if (!IsTribeOwner(tribeModel, ownerId))
            throw new HttpRequestException(HttpRequestError.Unknown,
                statusCode: HttpStatusCode.Forbidden, message: "Not enough rights");
        
        if (tribeModel.Participants.Select(p => p.Id).Contains(userId))
            throw new BadHttpRequestException("User already invited", 409, new ArgumentException());
        
        foreach (var member in leads.Concat(subordinates).ToArray())
        {
            var tribeMembers = tribeModel.Participants.ToArray();
            if (!tribeMembers.Select(x => x.Id).Contains(member))
                throw new HttpRequestException(HttpRequestError.Unknown,
                    statusCode: HttpStatusCode.BadRequest, message: $"{member} is not a member of tribe");
        }

        var appUser = userManager.Users.FirstOrDefault(x => x.Id == userId) ?? 
                      throw new HttpRequestException(HttpRequestError.Unknown,
                        statusCode: HttpStatusCode.NotFound, message: $"User not found");
        
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
        
        if (!IsTribeOwner(tribeModel, ownerId))
            // TODO: HttpRequestException - это все пятисотые
            throw new HttpRequestException(HttpRequestError.Unknown,
                statusCode: HttpStatusCode.Forbidden, message: "Not enough rights");
        
        if (ownerId == userId)
            throw new HttpRequestException(HttpRequestError.Unknown,
                statusCode: HttpStatusCode.BadRequest, message: "You cannot kick yourself from your own tribe");
        
        var appUser = userManager.Users.FirstOrDefault(x => x.Id == userId) ?? 
                      throw new HttpRequestException(HttpRequestError.Unknown,
                          statusCode: HttpStatusCode.NotFound, message: $"User not found");

        tribeModel.Participants.Remove(appUser);
        tribeModel.Positions = tribeModel.Positions.Where(x => x.UserId != appUser.Id).ToArray();
        
        await tribeRepository.UpdateAsync(tribeModel, cancellationToken);

        return true;
    }

    public async Task<bool> ChangeNameAsync(Guid tribeId, string newName, CancellationToken cancellationToken)
    {
        var ownerId = userService.GetUserIdOrThrow();
        var tribeModel = await GetByIdOrThrowAsync(tribeId, cancellationToken);
        
        if (!IsTribeOwner(tribeModel, ownerId))
            throw new HttpRequestException(HttpRequestError.Unknown,
                statusCode: HttpStatusCode.Forbidden, message: "Not enough rights");

        tribeModel.Name = newName;
        
        await tribeRepository.UpdateAsync(tribeModel, cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid tribeId, CancellationToken cancellationToken)
    {
        var ownerId = userService.GetUserIdOrThrow();
        var tribeModel = await GetByIdOrThrowAsync(tribeId, cancellationToken);
        
        if (!IsTribeOwner(tribeModel, ownerId))
            throw new HttpRequestException(HttpRequestError.Unknown,
                statusCode: HttpStatusCode.Forbidden, message: "Not enough rights");

        return await tribeRepository.DeleteAsync(tribeId, cancellationToken);
    }
    
    private async Task<TribeModel> GetByIdOrThrowAsync(Guid tribeId, CancellationToken cancellationToken)
    {
        return await tribeRepository.GetByIdAsync(tribeId, cancellationToken) ??
                    throw new HttpRequestException(HttpRequestError.Unknown,
                        statusCode: HttpStatusCode.NotFound, message: "Entity not found");
    }

    private static bool IsTribeOwner(TribeModel tribe, Guid userId) => tribe.CreatorId == userId;
}