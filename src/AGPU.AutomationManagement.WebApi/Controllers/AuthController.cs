using AGPU.AutomationManagement.Application.Auth;
using AGPU.AutomationManagement.Application.Auth.Commands;
using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.WebApi.Extensions;
using AGPU.AutomationManagement.WebApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AGPU.AutomationManagement.WebApi.Controllers;

public class AuthController : BaseController
{
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn(
        SignInRequest request,
        [FromServices] IUseCase<TokensDTO, SignInCommand> useCase,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await useCase.ExecuteAsync(request.ToCommand(), cancellationToken);
        return result.Match(e => Ok(e.ToResponse()), BadRequestWithProblemDetails);
    }
}