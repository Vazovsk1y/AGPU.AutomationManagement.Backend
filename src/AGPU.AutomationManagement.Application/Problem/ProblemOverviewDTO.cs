using AGPU.AutomationManagement.Domain.Enums;

namespace AGPU.AutomationManagement.Application.Problem;

public record ProblemOverviewDTO(
    Guid Id,
    string Title,
    DateTimeOffset CreationDateTime,
    string CreatorFullName,
    string Audience,
    ProblemStatus Status,
    ProblemType Type);