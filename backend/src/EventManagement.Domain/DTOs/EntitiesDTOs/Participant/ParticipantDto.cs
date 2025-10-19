namespace EventManagement.Domain.DTOs.EntitiesDTOs.Participant;

public record ParticipantDto(Guid UserId, string FullName, string Email, DateTime JoinedAt);