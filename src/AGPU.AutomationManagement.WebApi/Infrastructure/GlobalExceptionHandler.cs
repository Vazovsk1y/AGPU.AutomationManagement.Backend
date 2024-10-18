using Microsoft.AspNetCore.Diagnostics;

namespace AGPU.AutomationManagement.WebApi.Infrastructure;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}