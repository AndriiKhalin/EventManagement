using EventManagement.Infrastructure.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace EventManagement.Infrastructure;

public static class InfrastructureDI
{
    public static void AddInfrastructureServices(this IServiceCollection service)
    {
        service.ConfigureCors();
    }
}