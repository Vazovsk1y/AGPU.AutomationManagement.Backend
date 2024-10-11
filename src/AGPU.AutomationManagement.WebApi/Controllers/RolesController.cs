using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.Application.Role;
using AGPU.AutomationManagement.Application.Role.Queries;
using AGPU.AutomationManagement.WebApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AGPU.AutomationManagement.WebApi.Controllers;

public class RolesController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> RolesFetch(
        [FromServices] IUseCase<IReadOnlyCollection<RoleDTO>, RolesFetchQuery> useCase,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await useCase.HandleAsync(new RolesFetchQuery(), cancellationToken);
        return result.Match(
            e => Ok(e.Select(o => new RoleResponse(o.Id, o.Name))), 
            BadRequestWithProblemDetails);
    }
}