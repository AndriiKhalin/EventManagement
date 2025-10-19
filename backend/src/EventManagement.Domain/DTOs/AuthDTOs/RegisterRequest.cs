namespace EventManagement.Domain.DTOs.AuthDTOs;

public record RegisterRequest(string FirstName, string LastName, string Email, string Password);