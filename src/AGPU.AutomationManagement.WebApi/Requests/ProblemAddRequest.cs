using AGPU.AutomationManagement.Domain.Enums;

namespace AGPU.AutomationManagement.WebApi.Requests;

public record ProblemAddRequest(
    string Title,
    string Description,
    string Audience,
    ProblemType Type);