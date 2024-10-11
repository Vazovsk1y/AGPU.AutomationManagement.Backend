using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.Application.User.Commands;
using AGPU.AutomationManagement.WebApi.Extensions;
using AGPU.AutomationManagement.WebApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AGPU.AutomationManagement.WebApi.Controllers;

public class UsersController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> RegisterUser(
        UserRegisterRequest request,
        [FromServices] IUseCase<Guid, UserRegisterCommand> useCase,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await useCase.HandleAsync(request.ToRequest(), cancellationToken);
        return result.Match(Ok, BadRequestWithProblemDetails);
    }
}