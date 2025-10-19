namespace EventManagement.Domain.DTOs.EntitiesDTOs.Event;

public record UpdateEventDto(
    string Title,
    string Description,
    DateTime StartDateTime,
    string Location,
    int? Capacity,
    bool IsPublic);