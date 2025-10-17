using EventManagement.Domain.Entities;

namespace EventManagement.DataAccessLayer.DataSeed;

public static class UsersSeed
{
    public static readonly Guid JohnDoeId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid JaneSmithId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    private static readonly string HashPasswordJohn = "$2a$12$nk.AN9wr5mw8JaXoxTEhyuR2IdG0Eix/biFO28GwM3vCvoeiAU9g2";
    private static readonly string HashPasswordJane = "$2a$12$xmEAUayoOwVtj6sI2B/WweeeZGkJp49dCP/EQq6VZf4GzuYYqatQu";

    public static List<User> GetSeedUsers()
    {
        return
        [
            new User
            {
                Id = JohnDoeId,
                Email = "john@example.com",
                PasswordHash = HashPasswordJohn,
                FirstName = "John",
                LastName = "Doe",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },

            new User
            {
                Id = JaneSmithId,
                Email = "jane@example.com",
                PasswordHash = HashPasswordJane,
                FirstName = "Jane",
                LastName = "Smith",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        ];
    }
}