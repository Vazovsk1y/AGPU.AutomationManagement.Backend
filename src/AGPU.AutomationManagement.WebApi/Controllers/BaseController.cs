using System.Diagnostics;
using System.Net.Mime;
using AGPU.AutomationManagement.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace AGPU.AutomationManagement.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected IActionResult BadRequestWithProblemDetails(IEnumerable<Error> errors)
    {
        var result = new ValidationProblemDetails
        {
            Errors = new Dictionary<string, string[]> { { "ApplicationError", errors.Select(e => e.Message).ToArray() } },
            Status = 400,
            Title = "Bad request.",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            Extensions = new Dictionary<string, object?>
            {
                { "traceId", Activity.Current?.Id ?? HttpContext.TraceIdentifier },
            },
        };

        HttpContext.Response.ContentType = MediaTypeNames.Application.ProblemJson;
        return BadRequest(result);
    }
    
    protected static IActionResult Ok<T>(T value)
    {
        return new OkObjectResult(value);
    }
}