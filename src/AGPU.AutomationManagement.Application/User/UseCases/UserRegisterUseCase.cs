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
    UserManager<Domain.Entities.User> userManager,
    ICurrentUserProvider currentUserProvider) : IUseCase<Guid, UserRegisterCommand>
{
    public async Task<Result<Guid>> ExecuteAsync(UserRegisterCommand parameter, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var currentUser = await currentUserProvider.GetCurrentUserAsync();
        ArgumentNullException.ThrowIfNull(currentUser);
        
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
            var targetRole = await writeDbContext
                .Roles
                .FirstAsync(e => e.Id == parameter.RoleId, cancellationToken);
            
            if (targetRole.Name!.Equals(Roles.Administrator, StringComparison.InvariantCultureIgnoreCase) && 
                currentUser.Roles.All(e => e.RoleId != targetRole.Id))
            {
                return Result.Failure<Guid>("У вас недостаточно полномочий на выполнение данной операции.");
            }

            var userCreationResult = await userManager.CreateAsync(user, parameter.Password);
            if (!userCreationResult.Succeeded)
            {
                return userCreationResult.ToFailure<Guid>();
            }
            
            var addingToRoleResult = await userManager.AddToRoleAsync(user, targetRole.Name!);
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