namespace EventManagement.Domain.Settings;

public record JwtSettings(string Secret, string Issuer, string Audience, int ExpirationInMinutes = 60);