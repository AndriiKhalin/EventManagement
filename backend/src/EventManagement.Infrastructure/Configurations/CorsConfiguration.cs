using Microsoft.Extensions.DependencyInjection;

namespace EventManagement.Infrastructure.Configurations;

public static class CorsConfiguration
{
    public static void ConfigureCors(this IServiceCollection service)
    {
        service.AddCors(options =>
        {
            options.AddPolicy("AllowAngularApp", builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
    }
}