namespace Tribe.Api.Contracts.User.Responses;

public class GetUserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
}