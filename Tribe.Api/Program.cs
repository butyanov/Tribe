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

services.AddCors();

var app = builder.Build();

app.UseMiddleware<TransactionsMiddleware>();
app.UseMiddleware<ClientErrorsMiddleware>();

await app.Migrate();

app.MapIdentityApi<ApplicationUser>();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(option =>
{
    option.AllowAnyHeader();
    option.AllowAnyMethod();
    option.AllowCredentials();
    option.SetIsOriginAllowed(origin => true);
});

app.UseHttpsRedirection();

app.Run();