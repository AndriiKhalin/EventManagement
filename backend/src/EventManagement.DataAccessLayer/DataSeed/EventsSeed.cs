using EventManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.DataAccessLayer.DataSeed;

public class EventsSeed : IEntityTypeConfiguration<Event>
{
    public static readonly Guid TechConferenceId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    public static readonly Guid NetworkingMeetupId = Guid.Parse("44444444-4444-4444-4444-444444444444");
    public static readonly Guid DesignWorkshopId = Guid.Parse("55555555-5555-5555-5555-555555555555");

    public void Configure(EntityTypeBuilder<Event> builder)
    {
        var events = new List<Event>
        {
            new()
            {
                Id = TechConferenceId,
                Title = "Tech Conference 2025",
                Description =
                    "Annual technology conference featuring the latest innovations in AI and machine learning.",
                StartDateTime = new DateTime(2025, 04, 13, 11, 30, 0, DateTimeKind.Utc),
                Location = "Convention Center, San Francisco",
                Capacity = 500,
                IsPublic = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                OrganizerId = UsersSeed.JohnDoeId
            },
            new()
            {
                Id = NetworkingMeetupId,
                Title = "Community Networking Meetup",
                Description = "Connect with local professionals and expand your network.",
                StartDateTime = new DateTime(2025, 04, 12, 12, 0, 0, DateTimeKind.Utc),
                Location = "Downtown Coffee Shop",
                Capacity = 30,
                IsPublic = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                OrganizerId = UsersSeed.JaneSmithId
            },
            new()
            {
                Id = DesignWorkshopId,
                Title = "Design Workshop",
                Description = "Hands-on workshop covering modern UI/UX design principles.",
                StartDateTime = new DateTime(2025, 04, 10, 16, 30, 0, DateTimeKind.Utc),
                Location = "Creative Space Studio",
                Capacity = 20,
                IsPublic = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                OrganizerId = UsersSeed.JohnDoeId
            }
        };

        builder.HasData(events);
    }
}