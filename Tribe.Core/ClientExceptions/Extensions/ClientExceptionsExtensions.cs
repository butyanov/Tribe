using Microsoft.AspNetCore.Mvc;

namespace Tribe.Core.ClientExceptions.Extensions;

public static class ClientExceptionsExtensions
{
    public static ProblemDetails ToProblemDetails(this ClientException ex)
    {
        var problems = new ProblemDetails
        {
            Title = ex.Message,
            Status = ex.StatusCode,
            Extensions =
            {
                ["placeholderData"] = ex.PlaceholderData
            }
        };

        return problems;
    }
}