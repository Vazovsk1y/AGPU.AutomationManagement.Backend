﻿using AGPU.AutomationManagement.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace AGPU.AutomationManagement.Domain.Entities;

public sealed class User : IdentityUser<Guid>, IEntity
{
    public required string FullName { get; init; }
    
    public required string Post { get; init; }

    public IList<UserRole> Roles { get; init; } = new List<UserRole>();

    public IEnumerable<UserToken> Tokens { get; init; } = new List<UserToken>();
}