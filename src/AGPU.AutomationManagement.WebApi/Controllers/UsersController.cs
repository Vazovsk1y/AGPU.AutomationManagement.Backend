using AGPU.AutomationManagement.Application;
using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.Application.User;
using AGPU.AutomationManagement.Application.User.Commands;
using AGPU.AutomationManagement.Application.User.Queries;
using AGPU.AutomationManagement.Domain.Constants;
using AGPU.AutomationManagement.WebApi.Extensions;
using AGPU.AutomationManagement.WebApi.Infrastructure;
using AGPU.AutomationManagement.WebApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AGPU.AutomationManagement.WebApi.Controllers;


public class UsersController : BaseController
{
    [HttpPost]
    [ValidateSecurityStamp]
    [ValidateEmailConfirmation]
    [PermittedTo(Roles.Administrator, Roles.DeputyAdministrator)]
    public async Task<IActionResult> RegisterUser(
        UserRegisterRequest request,
        [FromServices] IUseCase<Guid, UserRegisterCommand> useCase,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await useCase.ExecuteAsync(request.ToCommand(), cancellationToken);
        return result.Match(Ok, BadRequestWithProblemDetails);
    }

    [HttpGet("contractors")]
    [ValidateSecurityStamp]
    [ValidateEmailConfirmation]
    [PermittedTo(Roles.Administrator, Roles.DeputyAdministrator)]
    public async Task<IActionResult> ContractorsFetch(
        [FromServices] IUseCase<IReadOnlyCollection<ContractorDTO>, ContractorsFetchQuery> useCase,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await useCase.ExecuteAsync(new ContractorsFetchQuery(), cancellationToken);
        return result.Match(
            e => Ok(e.Select(i => i.ToResponse())), 
            BadRequestWithProblemDetails);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me([FromServices] ICurrentUserProvider currentUserProvider, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var currentUser = await currentUserProvider.GetCurrentUserAsync();
        return currentUser is null ? Unauthorized() : Ok(currentUser.ToCurrentUserResponse());
    }
}