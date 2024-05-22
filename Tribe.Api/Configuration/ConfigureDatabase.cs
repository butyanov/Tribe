using Microsoft.EntityFrameworkCore;
using Npgsql;
using Tribe.Data;

namespace Tribe.Api.Configuration;

public static class ConfigureDatabase
{
    public static void AddDataContext(this WebApplicationBuilder builder)
    => builder.Services.AddDbContext<DataContext>(x =>
        x.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnectionString")));

    public static async Task Migrate(this WebApplication app)
    {
        try
        {
            await using var scope = app.Services.CreateAsyncScope();
            var sp = scope.ServiceProvider;
            var db = sp.GetRequiredService<DataContext>();
    
            await db.Database.MigrateAsync();

            await using var conn = (NpgsqlConnection)db.Database.GetDbConnection();
            await conn.OpenAsync();
            await conn.ReloadTypesAsync();
        }
        catch (Exception e)
        {
            app.Logger.LogError(e, "Error while migrating the database");
            Environment.Exit(-1);
        }
    }
}