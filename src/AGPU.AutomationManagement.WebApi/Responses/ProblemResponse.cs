using AGPU.AutomationManagement.Domain.Enums;

namespace AGPU.AutomationManagement.WebApi.Responses;

public record ProblemResponse(
    Guid Id,
    DateTimeOffset CreatedAt,
    Guid CreatorId,
    string CreatorFullName,
    string CreatorPost,
    ContractorResponse? Contractor,
    string Description,
    string Audience,
    DateTimeOffset? ExecutionDateTime,
    ProblemStatus Status,
    ProblemType Type
);