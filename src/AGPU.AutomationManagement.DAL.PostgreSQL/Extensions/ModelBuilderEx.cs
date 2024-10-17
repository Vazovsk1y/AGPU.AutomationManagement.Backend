using AGPU.AutomationManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.DAL.PostgreSQL.Extensions;

public static class ModelBuilderEx
{
    private static readonly Guid AdministratorRoleId = new("02627255-5d87-4006-957d-e27bcac894f3");
    private static readonly Guid UserRoleId = new("fc993a3b-506e-4d2e-b6e2-05f6d18012e1");
    private static readonly Guid DeputyAdministratorRoleId = new("87e462fc-d6c0-4d8f-9af0-c29a5bd9689a");
    private static readonly Guid EngineerRoleId = new("f26c547c-bdd6-40e2-a312-bc2f9df64ea3");

    private const string FirstUserPwd = "Admin_123#";
        
    private static readonly IEnumerable<Role> Roles =
    [
        new()
        {
            Id = AdministratorRoleId,
            Name = Domain.Constants.Roles.Administrator,
            ConcurrencyStamp = new Guid("fc993a3b-506e-4d2e-a6e2-05f6d18012e1").ToString(),
            NormalizedName = Domain.Constants.Roles.Administrator.ToUpper(),
        },
        new()
        {
            Id = DeputyAdministratorRoleId,
            Name = Domain.Constants.Roles.DeputyAdministrator,
            ConcurrencyStamp = new Guid("fc693a3b-506e-4d2e-a3e2-05f6d19012e1").ToString(),
            NormalizedName = Domain.Constants.Roles.DeputyAdministrator.ToUpper(),
        },
        new()
        {
            Id = EngineerRoleId,
            Name = Domain.Constants.Roles.Engineer,
            ConcurrencyStamp = new Guid("fb693a3b-506e-4d8e-a5e2-05f6d19012e1").ToString(),
            NormalizedName = Domain.Constants.Roles.Engineer.ToUpper(),
        },
        new()
        {
            Id = UserRoleId,
            Name = Domain.Constants.Roles.User,
            ConcurrencyStamp = new Guid("fc693a3b-506a-4d2e-a3e2-05f4b19012e1").ToString(),
            NormalizedName = Domain.Constants.Roles.User.ToUpper(),
        }
    ];
    
    internal static void Seed(this ModelBuilder builder)
    {
        builder.Entity<Role>().HasData(Roles);

        var firstUser = new User
        {
            FullName = "Самый Первый Админинистратор",
            Post = "Самый первый админ",
            Id = new Guid("3c04bbfc-9f26-444d-8028-9303e2e5f2e8"),
            Email = "test@gmail.com",
            EmailConfirmed = false,
            UserName = "SuperAdmin",
            NormalizedUserName = "SUPERADMIN",
            NormalizedEmail = "TEST@GMAIL.COM",
            ConcurrencyStamp = new Guid("3c04bbfd-9f26-444d-8028-9303e2e5f2e8").ToString(),
            SecurityStamp = new Guid("3c04bbfc-9f26-444d-8028-9303e2e5f2e6").ToString(),
        };

        var pwdHasher = new PasswordHasher<User>();
        firstUser.PasswordHash = pwdHasher.HashPassword(firstUser, FirstUserPwd);

        builder.Entity<User>().HasData(firstUser);

        builder.Entity<UserRole>().HasData(new UserRole()
        {
            RoleId = UserRoleId,
            UserId = firstUser.Id,
        }, new UserRole()
        {
            RoleId = AdministratorRoleId,
            UserId = firstUser.Id,
        }, new UserRole()
        {
            RoleId = DeputyAdministratorRoleId,
            UserId = firstUser.Id
        }, new UserRole()
        {
            RoleId = EngineerRoleId,
            UserId = firstUser.Id
        });
    }
}