using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.Application.Problem.Commands;
using AGPU.AutomationManagement.Application.Problem.Queries;
using AGPU.AutomationManagement.Domain.Enums;
using AGPU.AutomationManagement.WebApi.Extensions;
using AGPU.AutomationManagement.WebApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AGPU.AutomationManagement.WebApi.Controllers;

public class ProblemsController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> ProblemAdd(
        ProblemAddRequest request,
        [FromServices] IUseCase<Guid, ProblemAddCommand> useCase,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await useCase.ExecuteAsync(request.ToCommand(), cancellationToken);
        return result.Match(Ok, BadRequestWithProblemDetails);
    }

    [HttpGet("types")]
    public async Task<IActionResult> ProblemTypesFetch(
        [FromServices] IUseCase<IReadOnlyCollection<ProblemType>, ProblemTypesFetchQuery> useCase,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await useCase.ExecuteAsync(new ProblemTypesFetchQuery(), cancellationToken);
        return result.Match(e => Ok(e.Select(i => new { Value = (int)i, Name = i.ToString() })), BadRequestWithProblemDetails);
    }
}