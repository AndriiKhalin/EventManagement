using EventManagement.DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.DataAccessLayer.DataSeed;

public static class DatabaseSeeder
{
    public static async Task SeedDataAsync(ApplicationDbContext context)
    {
        await SeedUserDataAsync(context);
        await SeedEventDataAsync(context);
    }

    private static async Task SeedUserDataAsync(ApplicationDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var users = UsersSeed.GetSeedUsers();
        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();
    }

    private static async Task SeedEventDataAsync(ApplicationDbContext context)
    {
        if (await context.Events.AnyAsync()) return;

        var events = EventsSeed.GetSeedEvents();
        await context.Events.AddRangeAsync(events);
        await context.SaveChangesAsync();
    }
}