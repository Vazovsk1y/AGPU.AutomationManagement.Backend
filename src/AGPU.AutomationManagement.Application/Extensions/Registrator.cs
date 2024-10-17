using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using AGPU.AutomationManagement.Application.Auth.Services;
using AGPU.AutomationManagement.Application.Common;
using AGPU.AutomationManagement.Application.Infrastructure;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AGPU.AutomationManagement.Application.Extensions;

public static class Registrator
{
    public static IServiceCollection AddApplication(this IServiceCollection services, AuthSettings authSettings)
    {
        services.AddSingleton(authSettings);

        services.AddUseCases();
        
        services.Decorate(typeof(IUseCase<,>), typeof(ValidationUseCaseDecorator<,>));
        
        services.Decorate(typeof(IUseCase<>), typeof(ValidationUseCaseDecorator<>));
        
        services.AddSingleton(TimeProvider.System);

        services
            .AddIdentityCore<Domain.Entities.User>(e =>
            {
                e.User = new UserOptions { RequireUniqueEmail = true };
                e.ClaimsIdentity = authSettings.ClaimsIdentity;
            })
            .AddClaimsPrincipalFactory<AuthUserClaimsPrincipalFactory>()
            .AddTokenProvider<AccessTokenProvider>(AccessTokenProvider.Name)
            .AddTokenProvider<RefreshTokenProvider>(RefreshTokenProvider.Name)
            .AddRoles<Domain.Entities.Role>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager<SignInManager<Domain.Entities.User>>()
            .AddDefaultTokenProviders();

        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);
        
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Tokens.Access.SecretKey));

        JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services.AddAuthentication(e =>
            {
                e.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                e.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = authSettings.Tokens.Access.Issuer,
                    ValidAudience = authSettings.Tokens.Access.Audience,
                    IssuerSigningKey = signingKey,
                    ClockSkew = authSettings.Tokens.Access.ClockSkew,
                };
            });
        
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