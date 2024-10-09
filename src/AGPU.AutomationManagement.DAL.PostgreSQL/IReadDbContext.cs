﻿using AGPU.AutomationManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.DAL.PostgreSQL;

public interface IReadDbContext
{
    DbSet<IdentityUserToken<Guid>> UserTokens { get; }
    
    DbSet<IdentityRoleClaim<Guid>> RoleClaims { get; }
    
    DbSet<IdentityUserLogin<Guid>> UserLogins { get; }
    
    DbSet<IdentityUserClaim<Guid>> UserClaims { get; }
    
    DbSet<ProblemSolvingRequest> ProblemSolvingRequests { get; }

    DbSet<Role> Roles { get; }
    
    DbSet<User> Users { get; }
    
    DbSet<UserRole> UserRoles { get; }
}