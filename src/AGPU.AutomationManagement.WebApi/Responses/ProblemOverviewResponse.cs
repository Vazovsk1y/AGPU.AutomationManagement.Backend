using AGPU.AutomationManagement.Domain.Enums;

namespace AGPU.AutomationManagement.WebApi.Responses;

public record ProblemOverviewResponse(
    Guid Id,
    string Title,
    DateTimeOffset CreationDateTime,
    string CreatorFullName,
    ContractorResponse? Contractor,
    string Audience,
    DateTimeOffset? SolvingDateTime,
    ProblemStatus Status,
    ProblemType Type,
    float? SolvingScoreValue
);