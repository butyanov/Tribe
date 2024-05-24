using Microsoft.AspNetCore.Identity;
using Tribe.Data;
using Tribe.Domain.Models.User;

namespace Tribe.Api.Configuration;

public static class ConfigureAuth
{
    public static void AddAuth(this IServiceCollection services)
    {
        services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);
        services.AddAuthorizationBuilder();
        
        services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
                
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            })
            .AddEntityFrameworkStores<DataContext>()
            .AddApiEndpoints();
    }
}