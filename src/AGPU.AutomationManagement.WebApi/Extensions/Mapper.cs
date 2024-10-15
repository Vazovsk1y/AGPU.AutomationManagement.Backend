using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Problem;
using AGPU.AutomationManagement.Application.Problem.Commands;
using AGPU.AutomationManagement.Application.User;
using AGPU.AutomationManagement.Application.User.Commands;
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
            dto.CreatedAt,
            dto.CreatorId,
            dto.CreatorFullName,
            dto.CreatorPost,
            dto.Contractor?.ToResponse(),
            dto.Description,
            dto.Audience,
            dto.ExecutionDateTime,
            dto.Status,
            dto.Type
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

    public static ProblemAttachContractorCommand ToCommand(this ProblemAttachContractorRequest request)
    {
        return new ProblemAttachContractorCommand(
            request.ProblemId,
            request.ContractorId
        );
    }
}