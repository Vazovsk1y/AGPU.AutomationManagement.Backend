using AGPU.AutomationManagement.DAL.PostgreSQL.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AGPU.AutomationManagement.DAL.PostgreSQL.Extensions;

public static class Registrator
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, DatabaseSettings settings)
    {
        services.AddScoped<IWriteDbContext, ApplicationDbContext>(e =>
        {
            var loggerFactory = e.GetRequiredService<ILoggerFactory>();
            var builder = new DbContextOptionsBuilder();

            builder
                .UseNpgsql(settings.ConnectionString)
                .UseLoggerFactory(loggerFactory)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            
            return new ApplicationDbContext(builder.Options);
        });

        services.AddScoped<IReadDbContext, ApplicationDbContext>(e =>
        {
            var loggerFactory = e.GetRequiredService<ILoggerFactory>();
            var builder = new DbContextOptionsBuilder();

            builder
                .UseNpgsql(settings.ConnectionString)
                .UseLoggerFactory(loggerFactory)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            return new ApplicationDbContext(builder.Options);
        });

        // For IdentityFramework.
        services.AddDbContext<ApplicationDbContext>(e => e.UseNpgsql(settings.ConnectionString));

        return services;
    }
}