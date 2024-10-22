using AGPU.AutomationManagement.Application.User;
using AGPU.AutomationManagement.Domain.Enums;

namespace AGPU.AutomationManagement.Application.Problem;

public record ProblemDTO(
    Guid Id,
    string Title,
    DateTimeOffset CreationDateTime,
    string CreatorFullName,
    string CreatorPost,
    string CreatorUsername,
    ContractorDTO? Contractor,
    string Audience,
    DateTimeOffset? SolvingDateTime,
    ProblemStatus Status,
    ProblemType Type,
    float? SolvingScoreValue,
    string? SolvingScoreDescription);