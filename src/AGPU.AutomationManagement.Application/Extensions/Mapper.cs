using AGPU.AutomationManagement.Application.Problem;

namespace AGPU.AutomationManagement.Application.Extensions;

public static class Mapper
{
    public static ProblemDTO ToDTO(this Domain.Entities.Problem problem)
    {
        return new ProblemDTO(
            problem.Id,
            problem.CreatedAt,
            problem.CreatorId,
            problem.Creator.FullName,
            problem.Creator.Post,
            problem.Contractor?.ToContractorDTO(),
            problem.Description,
            problem.Audience,
            problem.ExecutionDateTime,
            problem.Status,
            problem.Type
        );
    }

    public static ContractorDTO ToContractorDTO(this Domain.Entities.User user)
    {
        return new ContractorDTO(
            user.Id,
            user.FullName,
            user.Post
        );
    }
}