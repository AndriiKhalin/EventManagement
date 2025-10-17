using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagement.Infrastructure.Configurations;

public static class ConnectionStringConfiguration
{
    public static void AddSecureDBPassword(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (connectionString != null)
        {
            // Replace ${DB_PASSWORD} with actual value from environment or user secrets
            connectionString = connectionString.Replace("{DB_PASSWORD}",
                configuration.GetSection("DB_PASSWORD").Value ?? "");

            // Update the configuration
            configuration["ConnectionStrings:DefaultConnection"] = connectionString;
        }
    }
}