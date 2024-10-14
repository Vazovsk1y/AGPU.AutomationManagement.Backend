using System.Reflection;
using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AGPU.AutomationManagement.Application.Extensions;

public static class Registrator
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddUseCases();
        
        services.Decorate(typeof(IUseCase<,>), typeof(ValidationUseCaseDecorator<,>));
        
        // services.Decorate(typeof(IUseCase<>), typeof(ValidationUseCaseDecorator<>));
        
        services.AddSingleton(TimeProvider.System);

        services
            .AddIdentityCore<Domain.Entities.User>(e =>
            {
                e.User = new UserOptions { RequireUniqueEmail = true };
            })
            .AddRoles<Domain.Entities.Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        return services;
    }

    private static void AddUseCases(this IServiceCollection services)
    {
        var useCaseTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } &&
                        t.GetInterfaces().Any(i => i.IsGenericType && !typeof(IDecorator).IsAssignableFrom(t) &&
                                                   (i.GetGenericTypeDefinition() == typeof(IUseCase<,>) || 
                                                    i.GetGenericTypeDefinition() == typeof(IUseCase<>))));

        foreach (var useCase in useCaseTypes)
        {
            var interfaces = useCase.GetInterfaces()
                .Where(i => i.IsGenericType &&
                            (i.GetGenericTypeDefinition() == typeof(IUseCase<,>) || 
                             i.GetGenericTypeDefinition() == typeof(IUseCase<>)));

            foreach (var @interface in interfaces)
            {
                services.AddScoped(@interface, useCase);
            }
        }
    }
}