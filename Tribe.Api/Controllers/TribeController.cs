using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tribe.Api.Contracts.Tribe.Requests;
using Tribe.Api.Contracts.Tribe.Responses;
using Tribe.Domain.Dto;
using Tribe.Domain.Facades;

namespace Tribe.Api.Controllers;

// TODO: Написать валидаторы
[ApiController]
[Route($"tribes")]
public class TribeController(ITribeFacade tribeFacade) : ControllerBase
{
    [Authorize]
    [HttpGet]
    [Route("get/{tribeId:guid}")]    
    public async Task<TribeResponse> GetMyTribe([FromRoute] Guid tribeId, CancellationToken cancellationToken)
    {
        var tribe = await tribeFacade.GetMyTribeAsync(tribeId, cancellationToken);

        var tribeResponse = new TribeResponse()
        {
            Id = tribe.Id,
            CreatorId = tribe.CreatorId,
            Name = tribe.Name,
            ParticipantsIds = tribe.ParticipantsIds,
            Positions = tribe.Positions
        };

        return tribeResponse;
    }
    [Authorize]
    [HttpGet]
    [Route("get-all")]    
    public async Task<IReadOnlyCollection<TribeResponse>> GetAllMyTribes(CancellationToken cancellationToken)
    {
        var tribes = await tribeFacade.GetAllMyTribesAsync(cancellationToken);
        
        var tribesResponse = tribes.Select(tribe => new TribeResponse()
        {
                Id = tribe.Id,
                CreatorId = tribe.CreatorId,
                Name = tribe.Name,
                ParticipantsIds = tribe.ParticipantsIds,
                Positions = tribe.Positions
        }).ToArray();

        return tribesResponse;
    }
    
    [Authorize]
    [HttpPost]
    [Route("create")]    
    public async Task<IActionResult> CreateTribe([FromBody] TribeRequest request, CancellationToken cancellationToken)
    {
        var tribeDto = new TribeDto
        {
            CreatorId = request.CreatorId,
            Name = request.Name,
            ParticipantsIds = request.ParticipantsIds,
            Positions = request.Positions
        };
        await tribeFacade.CreateAsync(tribeDto, cancellationToken);

        return Ok();
    }
    
    [Authorize]
    [HttpPut]
    [Route("add-user")]    
    public async Task<IActionResult> InviteUser([FromBody] InviteUserRequest request, CancellationToken cancellationToken)
    {
        await tribeFacade.AddUserAsync(request.TribeId, request.UserId, request.Leads, request.Subordinates, cancellationToken);
        return Ok();
    }
    
    [Authorize]
    [HttpPut]
    [Route("kick-user")]    
    public async Task<IActionResult> KickUser([FromBody] KickUserRequest request, CancellationToken cancellationToken)
    {
        await tribeFacade.KickUserAsync(request.TribeId, request.UserId, cancellationToken);
        return Ok();
    }
    
    [Authorize]
    [HttpPut]
    [Route("change-name")]    
    public async Task<IActionResult> KickUser([FromBody] ChangeNameRequest request, CancellationToken cancellationToken)
    {
        await tribeFacade.ChangeNameAsync(request.TribeId, request.NewName, cancellationToken);
        return Ok();
    }
    
    [Authorize]
    [HttpDelete]
    [Route("delete/{tribeId:guid}")]    
    public async Task<IActionResult> DeleteTribe([FromRoute] Guid tribeId, CancellationToken cancellationToken)
    {
        await tribeFacade.DeleteAsync(tribeId, cancellationToken);
        return Ok();
    }
}