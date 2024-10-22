using AGPU.AutomationManagement.Domain.Enums;

namespace AGPU.AutomationManagement.WebApi.Responses;

public record ProblemResponse(
    Guid Id,
    string Title,
    DateTimeOffset CreationDateTime,
    string CreatorFullName,
    string CreatorPost,
    ContractorResponse? Contractor,
    string Audience,
    DateTimeOffset? SolvingDateTime,
    string Description,
    ProblemStatus Status,
    ProblemType Type,
    float? SolvingScoreValue,
    string? SolvingScoreDescription);