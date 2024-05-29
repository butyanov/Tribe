using Tribe.Data.Configuration;
using Tribe.Domain.Database.Abstractions;

namespace Tribe.Api.Configuration;

public static class ConfigureEntitiesConfiguration
{
    public static void AddCustomEntitiesConfiguration(this IServiceCollection services)
    {
        services
            .AddSingleton<DependencyInjectedEntityConfiguration, TaskConfiguration>()
            .AddSingleton<DependencyInjectedEntityConfiguration, TribeConfiguration>();
    }
}