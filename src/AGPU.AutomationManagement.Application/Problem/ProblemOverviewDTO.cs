using AGPU.AutomationManagement.Application.User;
using AGPU.AutomationManagement.Domain.Enums;

namespace AGPU.AutomationManagement.Application.Problem;

public record ProblemOverviewDTO(
    Guid Id,
    string Title,
    DateTimeOffset CreationDateTime,
    string CreatorFullName,
    ContractorDTO? Contractor,
    string Audience,
    DateTimeOffset? SolvingDateTime,
    ProblemStatus Status,
    ProblemType Type,
    float? SolvingScoreValue);