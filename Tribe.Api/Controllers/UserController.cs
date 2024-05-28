using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tribe.Api.Contracts.User.Responses;
using Tribe.Domain.Models.User;
using Tribe.Domain.Services;

namespace Tribe.Api.Controllers;

[ApiController]
[Route($"users")]
public class UserController(UserManager<ApplicationUser> userManager, IUserService userService) : ControllerBase
{
    [Authorize]
    [HttpGet]
    [Route("me")]    
    public async Task<ActionResult<GetUserResponse>> GetMe(CancellationToken cancellationToken)
    {
        var userId  = userService.GetUserIdOrThrow();
        var user = await userManager.FindByIdAsync(userId.ToString());
        
        if (user == default)
            return NotFound();

        var userResponse = new GetUserResponse()
        {
            Id = user.Id,
            Email = user.Email!,
            Username = user.UserName!
        };
        
        return Ok(userResponse);
    }
    
    [Authorize]
    [HttpGet]
    [Route("by-ids")]    
    public async Task<ActionResult<IReadOnlyCollection<GetUserResponse>>> GetUsersByIds([FromQuery] IReadOnlyCollection<Guid> userIds, CancellationToken cancellationToken)
    {
        var users = new List<ApplicationUser?>();
        foreach (var userId in userIds)
        {
            users.Add(await userManager.FindByIdAsync(userId.ToString()));
        }
        
        if (users.Count == 0)
            return NotFound("No users found.");
        
        if (users.Any(u => u == null))
            return BadRequest();

        var userResponses = users.Select(u => new GetUserResponse
        {
            Id = u!.Id,
            Email = u.Email!,
            Username = u.UserName!
        });
        
        return Ok(userResponses);
    }
    
    [Authorize]
    [HttpGet]
    [Route("search-by-name")]    
    public async Task<ActionResult<IReadOnlyCollection<GetUserResponse>>> SearchUsers([FromQuery] string searchString, CancellationToken cancellationToken)
    {
        var users = await userManager.Users
            .Where(u => EF.Functions.Like(u.UserName!.ToUpper(), $"%{searchString.ToUpper()}%"))
            .Take(10)
            .ToListAsync(cancellationToken);
        
        if (users.Count == 0)
        {
            return NotFound("No users found.");
        }

        var userResponses = users.Select(u => new GetUserResponse
        {
            Id = u!.Id,
            Email = u.Email!,
            Username = u.UserName!
        });
        
        return Ok(userResponses);
    }
}