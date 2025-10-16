namespace EventManagement.Domain.Entities;

public class Event
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDateTime { get; set; }
    public string Location { get; set; } = string.Empty;
    public int? Capacity { get; set; }
    public bool IsPublic { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Foreign Keys
    public Guid OrganizerId { get; set; }

    // Navigation properties
    public User Organizer { get; set; } = null!;
    public ICollection<Participant> Participants { get; set; } = new List<Participant>();
}