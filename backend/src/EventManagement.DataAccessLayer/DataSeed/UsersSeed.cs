using EventManagement.Domain.Entities;

namespace EventManagement.DataAccessLayer.DataSeed;

public static class UsersSeed
{
    public static readonly Guid JohnDoeId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid JaneSmithId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    private static readonly string
        HashPasswordJohn = "$2a$12$Wh5EJIBlyE.d3BEgrWLy/.95HoxUR3QuOWjL4GYvdnRlZNd.H4yXW"; // andrew12345!

    private static readonly string
        HashPasswordJane = "$2a$12$nbjevSXAIfjvrk1DfpCyJO/RAmh8IipayN8qnIVns8xLQg1GGKn1q"; // andrew12345

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