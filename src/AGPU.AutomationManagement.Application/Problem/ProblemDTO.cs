using AGPU.AutomationManagement.Application.User;
using AGPU.AutomationManagement.Domain.Enums;

namespace AGPU.AutomationManagement.Application.Problem;

public record ProblemDTO(
    Guid Id,
    DateTimeOffset CreationDateTime,
    string CreatorFullName,
    string CreatorPost,
    ContractorDTO? Contractor,
    string Description,
    string Audience,
    DateTimeOffset? SolvingDateTime,
    ProblemStatus Status,
    ProblemType Type,
    float? SolvingScoreValue
    );