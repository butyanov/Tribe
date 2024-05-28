using Tribe.Api.Configuration;
using Tribe.Api.Middleware;
using Tribe.Core.Services;
using Tribe.Domain.Models.User;
using Tribe.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

builder.AddDataContext();
services.AddCustomEntitiesConfiguration();
services.AddRepositories();

services.AddAuth();
services.AddScoped<IUserService, UserService>();
services.AddFacades();
services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwagger();

services.AddCors(o =>
{
    o.AddDefaultPolicy(b =>
    {
        b.AllowAnyMethod()
            .AllowCredentials()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseMiddleware<TransactionsMiddleware>();
app.UseMiddleware<ValidationMiddleware>();

await app.Migrate();

app.MapIdentityApi<ApplicationUser>();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();