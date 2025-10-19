using EventManagement.DataAccessLayer.Data;
using EventManagement.Domain.Interfaces.Services.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.BusinessLogicLayer.Services.Entities.User;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task GetEventsAsync(Guid userId)
    {
        //var events = await _context.Events
        //    .Include(e => e.Organizer)
        //    .Include(e => e.Participants)
        //    .ThenInclude(p => p.User)
        //    .Where(e => e.OrganizerId == userId ||
        //                e.Participants.Any(p => p.UserId == userId))
        //    .OrderBy(e => e.StartDateTime)
        //    .ToListAsync();

        //return events.Select(e => MapToEventDto(e, userId)).ToList();
        throw new NotImplementedException();
    }


}