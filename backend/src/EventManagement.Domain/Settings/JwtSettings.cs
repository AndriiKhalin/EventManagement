namespace EventManagement.Domain.Settings;

//public class JwtSettings
//{
//    public string Secret { get; set; } = string.Empty;
//    public string Issuer { get; set; } = string.Empty;
//    public string Audience { get; set; } = string.Empty;
//    public int ExpirationInMinutes { get; set; } = 60;
//}

public record JwtSettings(string Secret, string Issuer, string Audience, int ExpirationInMinutes = 60);