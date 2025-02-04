using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.Application.Role;
using AGPU.AutomationManagement.Application.Role.Queries;
using AGPU.AutomationManagement.Domain.Constants;
using AGPU.AutomationManagement.WebApi.Infrastructure;
using AGPU.AutomationManagement.WebApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AGPU.AutomationManagement.WebApi.Controllers;

[ValidateSecurityStamp]
[ValidateEmailConfirmation]
public class RolesController : BaseController
{
    [HttpGet]
    [PermittedTo(Roles.Administrator, Roles.DeputyAdministrator)]
    public async Task<IActionResult> RolesFetch(
        [FromServices] IUseCase<IReadOnlyCollection<RoleDTO>, RolesFetchQuery> useCase,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await useCase.ExecuteAsync(new RolesFetchQuery(), cancellationToken);
        return result.Match(
            e => Ok(e.Select(o => new RoleResponse(o.Id, o.Name))),
            BadRequestWithProblemDetails);
    }
}