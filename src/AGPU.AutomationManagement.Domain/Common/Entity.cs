namespace AGPU.AutomationManagement.Domain.Common;

public abstract class Entity : IEntity
{
    public required Guid Id { get; init; } = Guid.NewGuid();
}