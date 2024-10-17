using System.Security.Cryptography;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AGPU.AutomationManagement.Application.Auth.Services;

internal sealed class RefreshTokenProvider(
    IServiceScopeFactory serviceScopeFactory)
    : IUserTwoFactorTokenProvider<Domain.Entities.User>
{
    public const string Name = "REFRESH_TOKEN";
    public const string LoginProvider = "AGPU.AUTOMATION_MANAGEMENT";

    public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<Domain.Entities.User> manager, Domain.Entities.User user) 
        => throw new NotImplementedException();

    public Task<string> GenerateAsync(string purpose, UserManager<Domain.Entities.User> manager, Domain.Entities.User user)
        => Task.FromResult(GenerateRandomString());

    public async Task<bool> ValidateAsync(string purpose, string token, UserManager<Domain.Entities.User> manager, Domain.Entities.User user)
    {
        if (purpose != Name)
        {
            throw new InvalidOperationException("Invalid refresh token purpose.");
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            return false;
        }

        using var scope = serviceScopeFactory.CreateScope();
        var timeProvider = scope.ServiceProvider.GetRequiredService<TimeProvider>();
        var dbContext = scope.ServiceProvider.GetRequiredService<IReadDbContext>();

        var refreshToken = await dbContext
            .UserTokens
            .SingleOrDefaultAsync(e => e.UserId == user.Id
                                       && e.LoginProvider == LoginProvider
                                       && e.Name == purpose);

        var currentDateTime = timeProvider.GetUtcNow();
        return refreshToken is not null && refreshToken.Value == token && refreshToken.Expires >= currentDateTime;
    }

    private static string GenerateRandomString()
    {
        var bytes = new byte[64];
        using var randomGenerator = RandomNumberGenerator.Create();
        randomGenerator.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}