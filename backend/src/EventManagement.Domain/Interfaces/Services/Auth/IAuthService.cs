using EventManagement.Domain.DTOs.AuthDTOs;
using EventManagement.Domain.DTOs.EntitiesDTOs.User;

namespace EventManagement.Domain.Interfaces.Services.Auth;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<UserDto?> GetCurrentUserAsync(Guid userId);
}