using System.Globalization;
using AGPU.AutomationManagement.Application.Auth;
using AGPU.AutomationManagement.Application.Auth.Commands;
using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Problem;
using AGPU.AutomationManagement.Application.Problem.Commands;
using AGPU.AutomationManagement.Application.User;
using AGPU.AutomationManagement.Application.User.Commands;
using AGPU.AutomationManagement.Domain.Entities;
using AGPU.AutomationManagement.WebApi.Requests;
using AGPU.AutomationManagement.WebApi.Responses;

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

    public static ProblemResponse ToResponse(this ProblemDTO dto)
    {
        return new ProblemResponse(
            dto.Id,
            dto.CreationDateTime,
            dto.CreatorFullName,
            dto.CreatorPost,
            dto.Contractor?.ToResponse(),
            dto.Description,
            dto.Audience,
            dto.SolvingDateTime,
            dto.Status,
            dto.Type,
            dto.SolvingScoreValue
        );
    }

    public static ContractorResponse ToResponse(this ContractorDTO dto)
    {
        return new ContractorResponse(
            dto.Id,
            dto.FullName,
            dto.Post
        );
    }
    
    public static PageResponse<TTo> ToPageResponse<TFrom, TTo>(this PageDTO<TFrom> dto, Func<TFrom, TTo> mapper)
    {
        return new PageResponse<TTo>
        {
            PageIndex = dto.PageIndex,
            Items = dto.Items.Select(mapper),
            TotalItemsCount = dto.TotalItemsCount,
            TotalPagesCount = dto.TotalPagesCount,
            HasNextPage = dto.HasNextPage,
            HasPreviousPage = dto.HasPreviousPage,
        };
    }

    public static ProblemAttachContractorCommand ToCommand(this ProblemAttachContractorRequest request, Guid problemId)
    {
        return new ProblemAttachContractorCommand(
            problemId,
            request.ContractorId
        );
    }

    public static ProblemMarkSolvedCommand ToCommand(this ProblemMarkSolvedRequest request, Guid problemId)
    {
        // TODO: Безопасное преобразование. Также нужно, чтобы конвертировало строки типа dd.mm.yyyy
        return new ProblemMarkSolvedCommand(
            problemId,
            new DateTimeOffset(
                DateOnly.Parse(request.SolvingDate, CultureInfo.InvariantCulture), 
                TimeOnly.Parse(request.SolvingTime, CultureInfo.InvariantCulture), 
                TimeSpan.Zero)
        );
    }

    public static SignInCommand ToCommand(this SignInRequest request)
    {
        return new SignInCommand(
            request.EmailOrUsername,
            request.Password
        );
    }

    public static TokensResponse ToResponse(this TokensDTO dto)
    {
        return new TokensResponse(
            dto.AccessToken, 
            dto.RefreshToken);
    }

    public static CurrentUserResponse ToCurrentUserResponse(this User user)
    {
        return new CurrentUserResponse(
            user.Id,
            user.UserName,
            user.FullName,
            user.Post,
            user.Email,
            user.EmailConfirmed,
            user.PhoneNumber,
            user.PhoneNumberConfirmed
        );
    }

    public static ProblemAssignSolvingScoreCommand ToCommand(this ProblemAssignSolvingScoreRequest request, Guid id)
    {
        return new ProblemAssignSolvingScoreCommand(id, request.Value, request.Description);
    }
}