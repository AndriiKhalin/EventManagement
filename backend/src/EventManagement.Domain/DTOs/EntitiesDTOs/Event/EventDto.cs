namespace EventManagement.Domain.DTOs.EntitiesDTOs.Event;

public record EventDto(
    Guid Id,
    string Title,
    string Description,
    DateTime StartDateTime,
    string Location,
    int? Capacity,
    int ParticipantCount,
    bool IsFull,
    bool IsUserParticipant
);