using AGPU.AutomationManagement.Application.Infrastructure.Settings;
using AGPU.AutomationManagement.DAL.PostgreSQL;
using AGPU.AutomationManagement.DAL.PostgreSQL.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AGPU.AutomationManagement.WebApi.Extensions;

public static class CommonEx
{
    public static void MigrateDatabase(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IWriteDbContext>();
        
        dbContext.Database.Migrate();
    }

    public static DatabaseSettings GetDatabaseSettings(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            var launchedFromDockerVariable = Environment.GetEnvironmentVariable("WEBAPI_LAUNCHED_FROM_DOCKER");
            var launchedFromDocker = !string.IsNullOrWhiteSpace(launchedFromDockerVariable) && bool.Parse(launchedFromDockerVariable);

            var sectionName = launchedFromDocker ? "DatabaseDocker" : "DatabaseDefault";
            var connectionString = builder.Configuration.GetConnectionString(sectionName) ?? throw new ApplicationException($"Строка подключения к базе данных для '{sectionName}' не определена.");
            return new DatabaseSettings { ConnectionString = connectionString };
        }
        
        throw new NotImplementedException();
    }
    
    public static ApplicationSettings GetApplicationSettings(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            var authSettings = builder.Configuration.GetRequiredSection("Auth").Get<AuthSettings>();
            ArgumentNullException.ThrowIfNull(authSettings);
            
            var keysPath = Environment.GetEnvironmentVariable("WEBAPI_DATA_PROTECTION_KEYS_PATH");
            ArgumentException.ThrowIfNullOrWhiteSpace(keysPath);
            
            return new ApplicationSettings { AuthSettings = authSettings, DataProtectionKeysPath = keysPath };
        }

        throw new NotImplementedException();
    }
}