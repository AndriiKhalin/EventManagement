namespace EventManagement.Domain.Entities;

public class Participant
{
    public Guid Id { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public Event Event { get; set; } = null!;
}