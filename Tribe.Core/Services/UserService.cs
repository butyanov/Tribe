using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Tribe.Domain.Services;

namespace Tribe.Core.Services;

public class UserService(IHttpContextAccessor httpContextAccessor) : IUserService
{
    public Guid? UserId => Guid.TryParse(
        httpContextAccessor.HttpContext
            ?.User.FindFirstValue(ClaimTypes.NameIdentifier),
        out var userId)
        ? userId
        : null;

    public bool IsAuthenticated => UserId != null;

    public Guid GetUserIdOrThrow() => UserId ?? throw new HttpRequestException(HttpRequestError.UserAuthenticationError,
        statusCode: HttpStatusCode.Unauthorized);
}