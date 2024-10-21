using AGPU.AutomationManagement.Domain.Enums;

namespace AGPU.AutomationManagement.WebApi.Responses;

public record ProblemResponse(
    Guid Id,
    DateTimeOffset CreationDateTime,
    string CreatorFullName,
    string CreatorPost,
    ContractorResponse? Contractor,
    string Description,
    string Audience,
    DateTimeOffset? SolvingDateTime,
    ProblemStatus Status,
    ProblemType Type,
    float? SolvingScoreValue
);