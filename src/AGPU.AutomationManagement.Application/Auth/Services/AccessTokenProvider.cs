using System.Security.Claims;
using System.Text;
using AGPU.AutomationManagement.Application.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AGPU.AutomationManagement.Application.Auth.Services;

internal sealed class AccessTokenProvider(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<AccessTokenProvider> logger,
    AuthSettings authSettings)
    : IUserTwoFactorTokenProvider<Domain.Entities.User>
{
    public const string Name = "ACCESS_TOKEN";
    public const string LoginProvider = "JWT";

    private readonly TokenSettings _tokenSettings = authSettings.Tokens;

    public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<Domain.Entities.User> manager, Domain.Entities.User user) 
        => throw new NotImplementedException();

    public async Task<string> GenerateAsync(string purpose, UserManager<Domain.Entities.User> manager, Domain.Entities.User user)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var claimsPrincipalFactory = scope.ServiceProvider.GetRequiredService<IUserClaimsPrincipalFactory<Domain.Entities.User>>();

        var claimsPrincipal = await claimsPrincipalFactory.CreateAsync(user);
        return GenerateJwtAccessToken(claimsPrincipal.Claims);
    }

    public async Task<bool> ValidateAsync(string purpose, string token, UserManager<Domain.Entities.User> manager, Domain.Entities.User user)
    {
        if (purpose != Name)
        {
            throw new InvalidOperationException("Invalid access token purpose.");
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            return false;
        }

        var claimsPrincipal = await GetClaimsPrincipalFromJwtToken(token);
        if (claimsPrincipal is null)
        {
            return false;
        }

        var userId = manager.GetUserId(claimsPrincipal);
        return !string.IsNullOrWhiteSpace(userId) && Guid.Parse(userId) == user.Id;
    }

    private string GenerateJwtAccessToken(IEnumerable<Claim> claims)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var timeProvider = scope.ServiceProvider.GetRequiredService<TimeProvider>();

        var currentDateTime = timeProvider.GetUtcNow().UtcDateTime;
        var expires = currentDateTime.Add(_tokenSettings.Access.TokenLifetime);
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Access.SecretKey)),
            SecurityAlgorithms.HmacSha256
            );
        
        var claimsDictionary = claims
            .GroupBy(c => c.Type)
            .ToDictionary(
                g => g.Key,
                g => g.Count() > 1 
                    ? g.Select(c => c.Value).ToArray()
                    : (object)g.First().Value);

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _tokenSettings.Access.Issuer,
            Audience = _tokenSettings.Access.Audience,
            Claims = claimsDictionary,
            Expires = expires,
            SigningCredentials = signingCredentials,
            NotBefore = currentDateTime,
            IssuedAt = currentDateTime,
        };

        var tokenValue = new JsonWebTokenHandler().CreateToken(descriptor);
        return tokenValue;
    }

    private async Task<ClaimsPrincipal?> GetClaimsPrincipalFromJwtToken(string token)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Access.SecretKey));
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            ValidIssuer = _tokenSettings.Access.Issuer,
            ValidAudience = _tokenSettings.Access.Audience,
            IssuerSigningKey = signingKey,
        };

        try
        {
            var handler = new JsonWebTokenHandler();
            var validationResult = await handler.ValidateTokenAsync(token, tokenValidationParameters);
            return validationResult.IsValid ? new ClaimsPrincipal(validationResult.ClaimsIdentity) : null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Возникло исключение во время валидации access токена.");
            return null;
        }
    }
}