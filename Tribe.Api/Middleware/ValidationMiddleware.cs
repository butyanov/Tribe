using Newtonsoft.Json;
using Tribe.Api.Contracts;
using ValidationException = FluentValidation.ValidationException;

namespace Tribe.Api.Middleware;

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