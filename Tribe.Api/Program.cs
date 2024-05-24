using Tribe.Api.Configuration;
using Tribe.Api.Middleware;
using Tribe.Domain.Models.User;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

builder.AddDataContext();

services.AddAuth();

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();