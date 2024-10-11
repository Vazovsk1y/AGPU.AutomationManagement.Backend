using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.Application.User.Commands;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using AGPU.AutomationManagement.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.Application.User.UseCases;

internal sealed class UserRegisterUseCase(
    IWriteDbContext writeDbContext,
    UserManager<Domain.Entities.User> userManager) : IUseCase<Guid, UserRegisterCommand>
{
    public async Task<Result<Guid>> HandleAsync(UserRegisterCommand parameter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        // TODO: Проверка ролей. Создать декоратор.
        
        await using var transaction = await writeDbContext.Database.BeginTransactionAsync(cancellationToken);
        var user = new Domain.Entities.User
        {
            FullName = parameter.FullName.Trim(),
            Post = parameter.Post.Trim(),
            UserName = parameter.Username.Trim(),
            Email = parameter.Email.Trim()
        };

        try
        {
            var userCreationResult = await userManager.CreateAsync(user, parameter.Password);
            if (!userCreationResult.Succeeded)
            {
                return userCreationResult.ToFailure<Guid>();
            }
            
            var targetRole = await writeDbContext.Roles.FirstAsync(e => e.Id == parameter.RoleId, cancellationToken);

            var addingToRoleResult = targetRole.Name!.Equals(Roles.User, StringComparison.InvariantCultureIgnoreCase)
                ? await userManager.AddToRoleAsync(user, Roles.User)
                : await userManager.AddToRolesAsync(user, [Roles.User, targetRole.Name]);

            if (!addingToRoleResult.Succeeded)
            {
                return addingToRoleResult.ToFailure<Guid>();
            }
            
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

        return user.Id;
    }
}