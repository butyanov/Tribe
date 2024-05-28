namespace Tribe.Domain.Services;

public interface IUserService
{
    public Guid? UserId { get; }

    public bool IsAuthenticated { get; }

    public Guid GetUserIdOrThrow();
}