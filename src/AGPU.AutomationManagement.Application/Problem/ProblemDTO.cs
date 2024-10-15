using AGPU.AutomationManagement.Application.User;
using AGPU.AutomationManagement.Domain.Enums;

namespace AGPU.AutomationManagement.Application.Problem;

public record ProblemDTO(
    Guid Id,
    DateTimeOffset CreatedAt,
    Guid CreatorId,
    string CreatorFullName,
    string CreatorPost,
    ContractorDTO? Contractor,
    string Description,
    string Audience,
    DateTimeOffset? ExecutionDateTime,
    ProblemStatus Status,
    ProblemType Type
    );