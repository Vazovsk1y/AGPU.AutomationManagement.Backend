using Microsoft.AspNetCore.Identity;

namespace AGPU.AutomationManagement.Domain.Entities;

public sealed class UserToken : IdentityUserToken<Guid>
{
    public DateTimeOffset? Expires { get; set; }
}