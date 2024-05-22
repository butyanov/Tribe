using Tribe.Api.Configuration;
using Tribe.Core.Models.User;
using Tribe.Infra.Middleware;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

builder.AddDataContext();

services.AddAuth();

services.AddEndpointsApiExplorer();
services.AddSwagger();

var app = builder.Build();

app.UseMiddleware<ValidationMiddleware>();

await app.Migrate();

app.MapIdentityApi<ApplicationUser>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();