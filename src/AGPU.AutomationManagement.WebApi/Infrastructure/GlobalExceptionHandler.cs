using System.Diagnostics;
using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.WebApi.Infrastructure;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger _logger = logger;
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        return exception switch
        { 
            OperationCanceledException operationCanceledException => await HandleOperationCanceledException(httpContext, operationCanceledException),
            DbUpdateException dbUpdateException => await HandleDbUpdateException(httpContext, dbUpdateException),
            _ => await HandleException(httpContext, exception),
        };
    }

    private async Task<bool> HandleException(HttpContext httpContext, Exception exception)
    {
        _logger.LogError(exception, "Something went wrong.");

        Activity.Current?.SetStatus(ActivityStatusCode.Error);
        // Activity.Current?.RecordException(exception);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal server error.",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
        };
        
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = MediaTypeNames.Application.ProblemJson;

        await httpContext.Response.WriteAsJsonAsync(problemDetails);

        return true;
    }

    private async Task<bool> HandleDbUpdateException(HttpContext httpContext, DbUpdateException exception)
    {
        _logger.LogError(exception, "An error occurred while saving changes to the database.");

        Activity.Current?.SetStatus(ActivityStatusCode.Error);
        // Activity.Current?.RecordException(exception);
        
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal server error.",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
        };

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = MediaTypeNames.Application.ProblemJson;

        await httpContext.Response.WriteAsJsonAsync(problemDetails);

        return true;
    }

    private async Task<bool> HandleOperationCanceledException(HttpContext httpContext, OperationCanceledException exception)
    {
        _logger.LogWarning(exception, "Operation was canceled.");
        
        httpContext.Response.StatusCode = StatusCodes.Status499ClientClosedRequest;
        httpContext.Response.ContentType = MediaTypeNames.Text.Plain;

        await httpContext.Response.WriteAsync("Выполнение операции было прервано.");

        return true;
    }
}