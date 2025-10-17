using EventManagement.DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagement.DataAccessLayer;

public static class DataAccessLayerDI
{
    public static void AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlContext(configuration);
        //services.ApplyMigrations();
    }

    private static void AddSqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptions => sqlServerOptions.MigrationsAssembly("EventManagement.DataAccessLayer")
            ));
    }
    //public static void ApplyMigrations(this IServiceCollection services)
    //{
    //    using var serviceProvider = services.BuildServiceProvider();
    //    using var scope = serviceProvider.CreateScope();
    //    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //    db.Database.Migrate();
    //}
}