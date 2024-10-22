using AGPU.AutomationManagement.Application.Problem;
using AGPU.AutomationManagement.Application.User;

namespace AGPU.AutomationManagement.Application.Extensions;

public static class Mapper
{
    public static ProblemDTO ToDTO(this Domain.Entities.Problem problem)
    {
        return new ProblemDTO(
            problem.Id,
            problem.Title,
            problem.CreationDateTime,
            problem.Creator.FullName,
            problem.Contractor?.ToContractorDTO(),
            problem.Audience,
            problem.SolvingDateTime,
            problem.Status,
            problem.Type,
            problem.SolvingScore?.Value
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