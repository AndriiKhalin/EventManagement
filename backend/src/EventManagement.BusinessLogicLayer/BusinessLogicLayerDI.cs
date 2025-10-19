using EventManagement.BusinessLogicLayer.Services.Auth;
using EventManagement.BusinessLogicLayer.Services.Entities.Event;
using EventManagement.Domain.Interfaces.Services.Auth;
using EventManagement.Domain.Interfaces.Services.Entities.Event;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using EventManagement.BusinessLogicLayer.Services.Entities.User;
using EventManagement.Domain.Interfaces.Services.Entities.User;

namespace EventManagement.BusinessLogicLayer;

public static class BusinessLogicLayerDI
{
    public static void RegisterBusinessLogicLayer(this IServiceCollection service)
    {
        service.AddScoped<IAuthService, AuthService>();
        service.AddScoped<IJwtService, JwtService>();
        service.AddScoped<IEventService, EventService>();
        service.AddScoped<IUserService, UserService>();
        service.AddFluentValidationAutoValidation();
        service.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}