using AGPU.AutomationManagement.Application.User;
using AGPU.AutomationManagement.Domain.Enums;

namespace AGPU.AutomationManagement.Application.Problem;

public record ProblemDTO(
    Guid Id,
    string Title,
    DateTimeOffset CreationDateTime,
    string CreatorFullName,
    string CreatorPost,
    ContractorDTO? Contractor,
    string Audience,
    DateTimeOffset? SolvingDateTime,
    string Description,
    ProblemStatus Status,
    ProblemType Type,
    float? SolvingScoreValue,
    string? SolvingScoreDescription);