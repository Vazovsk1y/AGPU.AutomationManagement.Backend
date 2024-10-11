using System.Diagnostics;
using System.Net.Mime;
using AGPU.AutomationManagement.Application;
using Microsoft.AspNetCore.Mvc;

namespace AGPU.AutomationManagement.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected IActionResult BadRequestWithProblemDetails(IEnumerable<Error> errors)
    {
        var responseErrors = errors
            .GroupBy(e => e.Code)
            .ToDictionary(e => e.Key, e => e.Select(t => t.Message).ToArray());

        var result = new ValidationProblemDetails
        {
            Errors = responseErrors,
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