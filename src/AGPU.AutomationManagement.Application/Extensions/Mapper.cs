using AGPU.AutomationManagement.Application.Problem;
using AGPU.AutomationManagement.Application.User;

namespace AGPU.AutomationManagement.Application.Extensions;

public static class Mapper
{
    public static ProblemOverviewDTO ToPreviewDTO(this Domain.Entities.Problem problem)
    {
        return new ProblemOverviewDTO(
            problem.Id,
            problem.Title,
            problem.CreationDateTime,
            problem.Creator.FullName,
            problem.Audience,
            problem.Status,
            problem.Type
        );
    }
    
    public static ProblemDTO ToDTO(this Domain.Entities.Problem problem)
    {
        return new ProblemDTO(
            problem.Id,
            problem.Title,
            problem.CreationDateTime,
            problem.Creator.FullName,
            problem.Creator.Post,
            problem.Contractor?.ToContractorDTO(),
            problem.Audience,
            problem.SolvingDateTime,
            problem.Description,
            problem.Status,
            problem.Type,
            problem.SolvingScore?.Value,
            problem.SolvingScore?.Description
        );
    }

    public static ContractorDTO ToContractorDTO(this Domain.Entities.User user)
    {
        return new ContractorDTO(
            user.Id,
            user.FullName,
            user.Post);
    }
}