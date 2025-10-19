using EventManagement.DataAccessLayer.Data;
using EventManagement.Domain.DTOs.EntitiesDTOs.Event;
using EventManagement.Domain.DTOs.EntitiesDTOs.Participant;
using EventManagement.Domain.DTOs.EntitiesDTOs.User;
using EventManagement.Domain.Entities;
using EventManagement.Domain.Interfaces.Services.Entities.Event;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.BusinessLogicLayer.Services.Entities.Event;

public class EventService : IEventService
{
    private readonly ApplicationDbContext _context;

    public EventService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<EventDto>> GetPublicEventsAsync(Guid? currentUserId)
    {
        var events = await _context.Events
            .Include(e => e.Organizer)
            .Include(e => e.Participants)
            .ThenInclude(p => p.User)
            .Where(e => e.IsPublic && e.StartDateTime > DateTime.UtcNow)
            .OrderBy(e => e.StartDateTime)
            .ToListAsync();

        return events.Select(e => MapToEventDto(e, currentUserId)).ToList();
    }

    public async Task<EventDetailsDto?> GetEventByIdAsync(Guid eventId, Guid? currentUserId)
    {
        var eventEntity = await _context.Events
            .Include(e => e.Organizer)
            .Include(e => e.Participants)
            .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(e => e.Id == eventId);

        return eventEntity != null ? MapToEventDetailsDto(eventEntity, currentUserId) : null;
    }

    public async Task<List<EventDto>> GetUserEventsAsync(Guid userId)
    {
        var events = await _context.Events
            .Include(e => e.Organizer)
            .Include(e => e.Participants)
            .ThenInclude(p => p.User)
            .Where(e => e.OrganizerId == userId ||
                        e.Participants.Any(p => p.UserId == userId))
            .OrderBy(e => e.StartDateTime)
            .ToListAsync();

        return events.Select(e => MapToEventDto(e, userId)).ToList();
    }

    public async Task<EventDetailsDto> CreateEventAsync(CreateEventDto request, Guid organizerId)
    {
        if (request.StartDateTime <= DateTime.UtcNow)
            throw new Exception("Event cannot be created in the past");

        var eventEntity = new Domain.Entities.Event
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            StartDateTime = request.StartDateTime,
            Location = request.Location,
            Capacity = request.Capacity,
            IsPublic = request.IsPublic,
            OrganizerId = organizerId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Events.Add(eventEntity);
        await _context.SaveChangesAsync();

        return await GetEventByIdAsync(eventEntity.Id, organizerId);
    }

    public async Task<EventDetailsDto> UpdateEventAsync(Guid eventId, UpdateEventDto request, Guid userId)
    {
        var eventEntity = await _context.Events
            .Include(e => e.Organizer)
            .FirstOrDefaultAsync(e => e.Id == eventId);

        if (eventEntity == null)
            throw new Exception("Event not found");

        if (eventEntity.OrganizerId != userId)
            throw new Exception("Only organizer can update the event");

        if (request.StartDateTime <= DateTime.UtcNow)
            throw new Exception("Event cannot be updated to a past date");

        eventEntity.Title = request.Title;
        eventEntity.Description = request.Description;
        eventEntity.StartDateTime = request.StartDateTime;
        eventEntity.Location = request.Location;
        eventEntity.Capacity = request.Capacity;
        eventEntity.IsPublic = request.IsPublic;
        eventEntity.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return (await GetEventByIdAsync(eventId, userId))!;
    }

    public async Task DeleteEventAsync(Guid eventId, Guid userId)
    {
        var eventEntity = await _context.Events.FindAsync(eventId);

        if (eventEntity == null)
            throw new Exception("Event not found");

        if (eventEntity.OrganizerId != userId)
            throw new Exception("Only organizer can delete the event");

        _context.Events.Remove(eventEntity);
        await _context.SaveChangesAsync();
    }

    public async Task JoinEventAsync(Guid eventId, Guid userId)
    {
        var eventEntity = await _context.Events
            .Include(e => e.Participants)
            .FirstOrDefaultAsync(e => e.Id == eventId);

        if (eventEntity == null)
            throw new Exception("Event not found");

        if (eventEntity.Participants.Any(p => p.UserId == userId))
            throw new Exception("User already joined this event");

        if (eventEntity.Capacity.HasValue &&
            eventEntity.Participants.Count >= eventEntity.Capacity.Value)
            throw new Exception("Event is full");

        var participant = new Participant
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            EventId = eventId
        };

        _context.Participants.Add(participant);
        await _context.SaveChangesAsync();
    }

    public async Task LeaveEventAsync(Guid eventId, Guid userId)
    {
        var participant = await _context.Participants
            .FirstOrDefaultAsync(p => p.EventId == eventId && p.UserId == userId);

        if (participant == null)
            throw new Exception("User is not a participant of this event");

        _context.Participants.Remove(participant);
        await _context.SaveChangesAsync();
    }

    private static EventDetailsDto MapToEventDetailsDto(Domain.Entities.Event eventEntity, Guid? currentUserId)
    {
        var isUserOrganizer = currentUserId.HasValue && eventEntity.OrganizerId == currentUserId.Value;

        return new EventDetailsDto(
            eventEntity.Id,
            eventEntity.Title,
            eventEntity.Description,
            eventEntity.StartDateTime,
            eventEntity.Location,
            eventEntity.Capacity,
            eventEntity.IsPublic,
            isUserOrganizer,
            new UserDto(eventEntity.Organizer.Id, eventEntity.Organizer.FirstName, eventEntity.Organizer.LastName,
                eventEntity.Organizer.Email),
            eventEntity.Participants.Select(p =>
                new ParticipantDto(p.User.Id, $"{p.User.FirstName} {p.User.LastName}", p.User.Email, p.JoinedAt)
            ));
    }

    private static EventDto MapToEventDto(Domain.Entities.Event eventEntity, Guid? currentUserId)
    {
        var participantCount = eventEntity.Participants.Count;
        var isFull = eventEntity.Capacity.HasValue && participantCount >= eventEntity.Capacity.Value;
        var isUserParticipant = currentUserId.HasValue &&
                                eventEntity.Participants.Any(p => p.UserId == currentUserId.Value);

        return new EventDto(eventEntity.Id, eventEntity.Title, eventEntity.Description, eventEntity.StartDateTime,
            eventEntity.Location, eventEntity.Capacity, participantCount, isFull, isUserParticipant);
    }
}