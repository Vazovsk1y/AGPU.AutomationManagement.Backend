using System.Reflection;
using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Role;
using AGPU.AutomationManagement.Application.Role.Queries;
using AGPU.AutomationManagement.Application.Role.UseCases;
using AGPU.AutomationManagement.Application.User.Commands;
using AGPU.AutomationManagement.Application.User.UseCases;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AGPU.AutomationManagement.Application.Extensions;

public static class Registrator
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUseCase<Guid, UserRegisterCommand>, UserRegisterUseCase>();
        services.Decorate<IUseCase<Guid, UserRegisterCommand>, ValidationUseCaseDecorator<Guid, UserRegisterCommand>>();

        services.AddScoped<IUseCase<IReadOnlyCollection<RoleDTO>, RolesFetchQuery>, RolesFetchUseCase>();

        services
            .AddIdentityCore<Domain.Entities.User>(e =>
            {
                e.User = new UserOptions { RequireUniqueEmail = true };
            })
            .AddRoles<Domain.Entities.Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        return services;
    }
}