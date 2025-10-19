namespace EventManagement.Domain.Interfaces.Services.Entities.User;

public interface IUserService
{
    public Task GetEventsAsync(Guid userId);
}