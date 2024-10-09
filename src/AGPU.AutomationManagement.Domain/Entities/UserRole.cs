using Microsoft.AspNetCore.Identity;

namespace AGPU.AutomationManagement.Domain.Entities;

public sealed class UserRole : IdentityUserRole<Guid>
{
    public User User { get; init; } = null!;

    public Role Role { get; init; } = null!;
}