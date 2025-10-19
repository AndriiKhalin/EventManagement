namespace EventManagement.Domain.DTOs.EntitiesDTOs.Event;

public record CreateEventDto(
    string Title,
    string Description,
    DateTime StartDateTime,
    string Location,
    int? Capacity,
    bool IsPublic);