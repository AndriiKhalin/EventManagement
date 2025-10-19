using EventManagement.Domain.DTOs.EntitiesDTOs.Participant;
using EventManagement.Domain.DTOs.EntitiesDTOs.User;

namespace EventManagement.Domain.DTOs.EntitiesDTOs.Event;

public record EventDetailsDto(
    Guid Id,
    string Title,
    string Description,
    DateTime StartDateTime,
    string Location,
    int? Capacity,
    bool IsPublic,
    bool IsUserOrganizer,
    UserDto Organizer,
    IEnumerable<ParticipantDto> Participants
);