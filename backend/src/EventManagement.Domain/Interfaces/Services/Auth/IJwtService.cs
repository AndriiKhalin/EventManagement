using EventManagement.Domain.Entities;

namespace EventManagement.Domain.Interfaces.Services.Auth;

public interface IJwtService
{
    string GenerateJwtToken(User user);
}