using medicine_box_api.Domain.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace medicine_box_api.Infraestructure.Configuration;
public static class DatabaseConfig
{
    public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks();

        var dbConfig = configuration
            .GetSection("ConnectionStrings")
            .Get<DatabaseConfiguration>()!;

        var host = dbConfig.Host;
        var database = dbConfig.Database;
        var username = dbConfig.Username;
        var password = dbConfig.Password;

        var connectionString = $"host={host};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=True";

        services.AddDbContext<dbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
        });

        services.AddHealthChecks()
            .AddNpgSql(connectionString, name: "postgre-supabase");
    }
}
