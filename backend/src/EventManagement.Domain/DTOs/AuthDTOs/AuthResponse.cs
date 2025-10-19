using EventManagement.Domain.DTOs.EntitiesDTOs.User;

namespace EventManagement.Domain.DTOs.AuthDTOs;

public record AuthResponse(string Token, UserDto User);