using AGPU.AutomationManagement.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace AGPU.AutomationManagement.Domain.Entities;

public sealed class Role : IdentityRole<Guid>, IEntity
{
    
}