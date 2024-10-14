using AGPU.AutomationManagement.Application.Problem.Commands;
using AGPU.AutomationManagement.Application.User.Commands;
using AGPU.AutomationManagement.WebApi.Requests;

namespace AGPU.AutomationManagement.WebApi.Extensions;

public static class Mapper
{
    public static UserRegisterCommand ToCommand(this UserRegisterRequest request)
    {
        return new UserRegisterCommand(
            request.Username,
            request.Password,
            request.FullName,
            request.Post,
            request.Email,
            request.RoleId
        );
    }

    public static ProblemAddCommand ToCommand(this ProblemAddRequest request)
    {
        return new ProblemAddCommand(
            request.Description,
            request.Audience,
            request.Type
        );
    }
}