using System.Security.Claims;
using EventManagement.Domain.DTOs.EntitiesDTOs.Event;
using EventManagement.Domain.Interfaces.Services.Entities.Event;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    [HttpGet]
    public async Task<ActionResult<List<EventDto>>> GetPublicEvents()
    {
        var events = await _eventService.GetPublicEventsAsync(GetCurrentUserId());
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> GetEvent(Guid id)
    {
        var eventDto = await _eventService.GetEventByIdAsync(id, GetCurrentUserId());
        return eventDto != null ? Ok(eventDto) : NotFound();
    }

    [Authorize]
    [HttpGet("my-events")]
    public async Task<ActionResult<List<EventDto>>> GetMyEvents()
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue) return Unauthorized();

        var events = await _eventService.GetUserEventsAsync(userId.Value);
        return Ok(events);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<EventDto>> CreateEvent([FromBody] CreateEventDto request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue) return Unauthorized();

            var eventDto = await _eventService.CreateEventAsync(request, userId.Value);
            return CreatedAtAction(nameof(GetEvent), new { id = eventDto.Id }, eventDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPatch("{id}")]
    public async Task<ActionResult<EventDto>> UpdateEvent(Guid id, [FromBody] UpdateEventDto request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue) return Unauthorized();

            var eventDto = await _eventService.UpdateEventAsync(id, request, userId.Value);
            return Ok(eventDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEvent(Guid id)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue) return Unauthorized();

            await _eventService.DeleteEventAsync(id, userId.Value);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("{id}/join")]
    public async Task<ActionResult> JoinEvent(Guid id)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue) return Unauthorized();

            await _eventService.JoinEventAsync(id, userId.Value);
            return Ok(new { message = "Successfully joined the event" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("{id}/leave")]
    public async Task<ActionResult> LeaveEvent(Guid id)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue) return Unauthorized();

            await _eventService.LeaveEventAsync(id, userId.Value);
            return Ok(new { message = "Successfully left the event" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}