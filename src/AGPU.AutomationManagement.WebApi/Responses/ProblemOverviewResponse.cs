using AGPU.AutomationManagement.Domain.Enums;

namespace AGPU.AutomationManagement.WebApi.Responses;

public record ProblemOverviewResponse(
    Guid Id,
    string Title,
    DateTimeOffset CreationDateTime,
    string CreatorFullName,
    string Audience,
    ProblemStatus Status,
    ProblemType Type);