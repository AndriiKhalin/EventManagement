using EventManagement.DataAccessLayer.Data;
using EventManagement.Domain.DTOs.AuthDTOs;
using EventManagement.Domain.DTOs.EntitiesDTOs.User;
using EventManagement.Domain.Entities;
using EventManagement.Domain.Interfaces.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.BusinessLogicLayer.Services.Auth;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;

    private readonly IJwtService _jwtService;

    public AuthService(ApplicationDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            throw new Exception("User with this email already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = _jwtService.GenerateJwtToken(user);

        return new AuthResponse(token, MapToUserDto(user));
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new Exception("Invalid email or password");

        var token = _jwtService.GenerateJwtToken(user);

        return new AuthResponse(token, MapToUserDto(user));
    }

    public async Task<UserDto?> GetCurrentUserAsync(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        return user != null ? MapToUserDto(user) : null;
    }

    private static UserDto MapToUserDto(User user)
    {
        return new UserDto(user.Id, user.FirstName, user.LastName, user.Email);
    }
}