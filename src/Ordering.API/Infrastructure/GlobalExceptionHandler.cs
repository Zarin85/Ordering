using Microsoft.AspNetCore.Diagnostics;
using Ordering.Application.Orders.Exceptions;
using Ordering.Domain.Common;

namespace Ordering.API.Infrastructure;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken ct)
    {
        var (statusCode, message) = exception switch
        {
            DomainException => (StatusCodes.Status422UnprocessableEntity, exception.Message),
            NotFoundException => (StatusCodes.Status404NotFound, exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(new { error = message }, ct);
        return true;
    }
}
