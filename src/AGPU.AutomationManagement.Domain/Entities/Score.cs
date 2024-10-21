using AGPU.AutomationManagement.Domain.Common;

namespace AGPU.AutomationManagement.Domain.Entities;

public sealed class Score : Entity
{
    public required DateTimeOffset CreatedAt { get; init; }
    
    public required Guid ProblemId { get; init; }
    
    public required float Value { get; init; }
    
    public string? Description { get; init; }

    public Problem Problem { get; init; } = null!;
}