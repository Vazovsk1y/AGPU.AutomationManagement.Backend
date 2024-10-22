using System.ComponentModel.DataAnnotations;
using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.Application.Problem;
using AGPU.AutomationManagement.Application.Problem.Commands;
using AGPU.AutomationManagement.Application.Problem.Queries;
using AGPU.AutomationManagement.Domain.Constants;
using AGPU.AutomationManagement.Domain.Enums;
using AGPU.AutomationManagement.WebApi.Extensions;
using AGPU.AutomationManagement.WebApi.Infrastructure;
using AGPU.AutomationManagement.WebApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AGPU.AutomationManagement.WebApi.Controllers;

[ValidateSecurityStamp]
[ValidateEmailConfirmation]
public class ProblemsController : BaseController
{
    [HttpPost]
    [PermittedTo(Roles.User)]
    public async Task<IActionResult> ProblemAdd(
        ProblemAddRequest request,
        [FromServices] IUseCase<Guid, ProblemAddCommand> useCase,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await useCase.ExecuteAsync(request.ToCommand(), cancellationToken);
        return result.Match(Ok, BadRequestWithProblemDetails);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ProblemsPageFetch(
        [FromQuery] [Range(1, int.MaxValue)] int pageIndex, 
        [FromQuery] [Range(1, int.MaxValue)] int pageSize,
        [FromQuery] ProblemStatus? status,
        [FromServices] IUseCase<PageDTO<ProblemDTO>, ProblemsPageFetchQuery> useCase,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await useCase.ExecuteAsync(new ProblemsPageFetchQuery(
            new PagingOptions(pageIndex, pageSize), 
            new ProblemsPageFilters(status)), cancellationToken);
        return result.Match(e => Ok(e.ToPageResponse(i => i.ToResponse())), BadRequestWithProblemDetails);
    }

    [HttpPatch("{id}/attach-contractor")]
    [PermittedTo(Roles.Administrator, Roles.DeputyAdministrator)]
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

    [HttpPatch("{id}/mark-solved")]
    [PermittedTo(Roles.Engineer)]
    public async Task<IActionResult> ProblemMarkSolved(
        [FromRoute] Guid id,
        ProblemMarkSolvedRequest request,
        [FromServices] IUseCase<ProblemMarkSolvedCommand> useCase,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!request.TryToCommand(id, out var command))
        {
            return BadRequest();
        }

        var result = await useCase.ExecuteAsync(command, cancellationToken);
        return result.Match(Ok, BadRequestWithProblemDetails);
    }

    [HttpPatch("{id}/assign-solving-score")]
    [PermittedTo(Roles.User)]
    public async Task<IActionResult> ProblemAssignSolvingScore(
        [FromRoute] Guid id,
        ProblemAssignSolvingScoreRequest request,
        [FromServices] IUseCase<ProblemAssignSolvingScoreCommand> useCase,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await useCase.ExecuteAsync(request.ToCommand(id), cancellationToken);
        return result.Match(Ok, BadRequestWithProblemDetails);
    }
}