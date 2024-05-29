using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Tribe.Api.Contracts;
using Tribe.Core.ClientExceptions;
using Tribe.Core.ClientExceptions.Extensions;
using ValidationException = FluentValidation.ValidationException;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Tribe.Api.Middleware;

public class ClientErrorsMiddleware(RequestDelegate request, IHostEnvironment hostingEnvironment)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await request(context);
        }
        catch (ClientException ex)
        {
            await WriteDomainError(context, ex);
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

    private Task WriteDomainError(HttpContext context, ClientException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception.StatusCode;

        var problem = exception.ToProblemDetails();

        if (!hostingEnvironment.IsProduction())
            problem.Extensions.Add("trace", exception.StackTrace);
        problem.Extensions.Add("traceId", context.TraceIdentifier);

        var jsonOptions = context.RequestServices.GetRequiredService<IOptions<JsonOptions>>();

        var settings = new JsonSerializerSettings
        {
            Formatting = jsonOptions.Value.SerializerOptions.WriteIndented ? Formatting.Indented : Formatting.None
        };

        var json = JsonConvert.SerializeObject(problem, settings);

        return context.Response.WriteAsync(json);
    }
}