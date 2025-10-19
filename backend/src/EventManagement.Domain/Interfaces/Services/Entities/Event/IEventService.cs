using EventManagement.Domain.DTOs.EntitiesDTOs.Event;

namespace EventManagement.Domain.Interfaces.Services.Entities.Event;

public interface IEventService
{
    Task<List<EventDto>> GetPublicEventsAsync(Guid? currentUserId);
    Task<EventDetailsDto?> GetEventByIdAsync(Guid eventId, Guid? currentUserId);
    Task<List<EventDto>> GetUserEventsAsync(Guid userId);
    Task<EventDetailsDto> CreateEventAsync(CreateEventDto request, Guid organizerId);
    Task<EventDetailsDto> UpdateEventAsync(Guid eventId, UpdateEventDto request, Guid userId);
    Task DeleteEventAsync(Guid eventId, Guid userId);
    Task JoinEventAsync(Guid eventId, Guid userId);
    Task LeaveEventAsync(Guid eventId, Guid userId);
}