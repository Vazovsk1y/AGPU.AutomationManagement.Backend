using AGPU.AutomationManagement.Application.Extensions;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using AGPU.AutomationManagement.Domain.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.Application.User.UseCases;

public interface IRegisterUserUseCase
{
    Task<Result<Guid>> HandleAsync(UserRegisterDTO dto, CancellationToken cancellationToken);
}

internal sealed class RegisterUserUseCase(
    IWriteDbContext writeDbContext,
    UserManager<Domain.Entities.User> userManager,
    IValidator<UserRegisterDTO> validator) : IRegisterUserUseCase
{
    public async Task<Result<Guid>> HandleAsync(UserRegisterDTO dto, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        // TODO: Проверка ролей. Создать декоратор.
        
        // TODO: Создать декоратор для валидации.
        var validationResult = await validator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToFailure<Guid>();
        }
        
        await using var transaction = await writeDbContext.Database.BeginTransactionAsync(cancellationToken);
        var user = new Domain.Entities.User
        {
            FullName = dto.FullName.Trim(),
            Post = dto.Post.Trim(),
            UserName = dto.Username.Trim(),
            Email = dto.Email.Trim()
        };

        try
        {
            // TODO: Указать уникальность емейла.
            var userCreationResult = await userManager.CreateAsync(user, dto.Password);
            if (!userCreationResult.Succeeded)
            {
                return userCreationResult.ToFailure<Guid>();
            }
            
            var targetRole = await writeDbContext.Roles.FirstAsync(e => e.Id == dto.RoleId, cancellationToken);

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