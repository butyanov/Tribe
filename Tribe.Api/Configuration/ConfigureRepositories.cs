using Tribe.Core.Repositories;
using Tribe.Domain.Repositories;

namespace Tribe.Api.Configuration;

public static class ConfigureRepositories
{
    public static void AddRepositories(this IServiceCollection services) =>
        services
            .AddScoped<ITribeRepository, TribeRepository>()
            .AddScoped<ITaskRepository, TaskRepository>();
}