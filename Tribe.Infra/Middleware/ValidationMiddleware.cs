using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Tribe.Infra.Contracts.Responses;

namespace Tribe.Infra.Middleware;

public class ValidationMiddleware(RequestDelegate request)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await request(context);
        }
        catch (ValidationException exception)
        {
            context.Response.StatusCode = 400;
            var messages = exception.Errors.Select(x => x.ErrorMessage).ToList();
            var validationFailureResponse = new ValidationFailureResponse
            {
                Errors = messages
            };
            
            var response = JsonConvert.SerializeObject(validationFailureResponse);
            await context.Response.WriteAsync(response);
        }
    }
}