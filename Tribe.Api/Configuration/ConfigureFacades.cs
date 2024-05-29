using Tribe.Core.Facades;
using Tribe.Domain.Facades;

namespace Tribe.Api.Configuration;

public static class ConfigureFacades
{
    public static void AddFacades(this IServiceCollection services)
    {
        services
            .AddScoped<ITribeFacade, TribeFacade>()
            .AddScoped<ITaskFacade, TaskFacade>();
    }
}