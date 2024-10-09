using AGPU.AutomationManagement.Domain.Common;

namespace AGPU.AutomationManagement.Domain.Entities;

public sealed class Problem : Entity
{
    public required string Name { get; init; }
}