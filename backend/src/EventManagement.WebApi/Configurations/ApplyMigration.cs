using EventManagement.DataAccessLayer.Data;
using EventManagement.DataAccessLayer.DataSeed;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.WebApi.Configurations;

public static class ApplyMigration
{
    public static async Task MigrateAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        await db.Database.MigrateAsync();

        var environment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        logger.LogInformation("Current environment: {Environment}", environment.EnvironmentName);

        if (environment.IsDevelopment())
        {
            logger.LogInformation("Seeding development data");
            await DatabaseSeeder.SeedDataAsync(db);
        }

        logger.LogInformation("Skipping data seeding - not in Development environment");
    }
}