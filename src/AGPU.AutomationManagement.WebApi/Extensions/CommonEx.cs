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
            var connectionString = builder.Configuration.GetConnectionString("Default");
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
            
            return new DatabaseSettings()
            {
                ConnectionString = connectionString,
            };
        }
        
        throw new NotImplementedException();
    }
    
    public static AuthSettings GetAuthSettings(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            var authSettings = builder.Configuration.GetRequiredSection("Auth").Get<AuthSettings>();
            ArgumentNullException.ThrowIfNull(authSettings);
            
            return authSettings;
        }

        throw new NotImplementedException();
    }
}