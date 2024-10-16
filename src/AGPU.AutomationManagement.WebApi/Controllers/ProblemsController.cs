using System.ComponentModel.DataAnnotations;
using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.Application.Problem;
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

    [HttpGet]
    public async Task<IActionResult> ProblemsPageFetch(
        [FromQuery] [Range(1, int.MaxValue)] int pageIndex, 
        [FromQuery] [Range(1, int.MaxValue)] int pageSize,
        [FromServices] IUseCase<PageDTO<ProblemDTO>, ProblemsPageFetchQuery> useCase,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await useCase.ExecuteAsync(new ProblemsPageFetchQuery(new PagingOptions(pageIndex, pageSize)), cancellationToken);
        return result.Match(e => Ok(e.ToPageResponse(i => i.ToResponse())), BadRequestWithProblemDetails);
    }

    [HttpPatch("{id}/attach-contractor")]
    public async Task<IActionResult> ProblemAttachContractor(
        [FromRoute] Guid id,
        ProblemAttachContractorRequest request,
        [FromServices] IUseCase<ProblemAttachContractorCommand> useCase,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await useCase.ExecuteAsync(request.ToCommand(id), cancellationToken);
        return result.Match(Ok, BadRequestWithProblemDetails);
    }

    [HttpPatch("{id}/mark-completed")]
    public async Task<IActionResult> ProblemMarkCompleted(
        [FromRoute] Guid id,
        ProblemMarkCompletedRequest request,
        [FromServices] IUseCase<ProblemMarkCompletedCommand> useCase,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await useCase.ExecuteAsync(request.ToCommand(id), cancellationToken);
        return result.Match(Ok, BadRequestWithProblemDetails);
    }
}