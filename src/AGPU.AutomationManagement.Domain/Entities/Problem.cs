using AGPU.AutomationManagement.Domain.Common;
using AGPU.AutomationManagement.Domain.Enums;

namespace AGPU.AutomationManagement.Domain.Entities;

public sealed class Problem : Entity
{
    public required DateTimeOffset CreatedAt { get; init; }
    
    public required Guid CreatorId { get; init; }
    
    public Guid? ContractorId { get; set; }
    
    public required string Description { get; init; }
    
    public required string Audience { get; init; }
    
    public DateTimeOffset? ExecutionDateTime { get; set; }
    
    public required ProblemStatus Status { get; set; }
    
    public required ProblemType Type { get; init; }
    
    public User Creator { get; init; } = null!;

    public User? Contractor { get; init; }
    
    public Score? Score { get; init; }
}