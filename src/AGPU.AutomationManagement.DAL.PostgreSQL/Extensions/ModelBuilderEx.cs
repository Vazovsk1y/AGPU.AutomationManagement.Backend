using AGPU.AutomationManagement.Domain.Constants;
using AGPU.AutomationManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.DAL.PostgreSQL.Extensions;

public static class ModelBuilderEx
{
    private static readonly IEnumerable<Role> Roles =
    [
        new()
        {
            Id = Guid.NewGuid(),
            Name = Domain.Constants.Roles.Administrator,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            NormalizedName = Domain.Constants.Roles.Administrator.ToUpper(),
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = Domain.Constants.Roles.DeputyAdministrator,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            NormalizedName = Domain.Constants.Roles.DeputyAdministrator.ToUpper(),
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = Domain.Constants.Roles.Engineer,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            NormalizedName = Domain.Constants.Roles.Engineer.ToUpper(),
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = Domain.Constants.Roles.User,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            NormalizedName = Domain.Constants.Roles.User.ToUpper(),
        }
    ];
    
    internal static void Seed(this ModelBuilder builder)
    {
        builder.Entity<Role>().HasData(Roles);
    }
}