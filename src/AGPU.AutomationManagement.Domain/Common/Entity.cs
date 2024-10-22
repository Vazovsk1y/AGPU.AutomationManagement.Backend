namespace AGPU.AutomationManagement.Domain.Common;

public abstract class Entity : IEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
}