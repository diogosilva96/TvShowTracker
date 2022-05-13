using Microsoft.EntityFrameworkCore;
using TvShowTracker.DataAccessLayer;
using TvShowTracker.DataAccessLayer.Models;

namespace TvShowTracker.Background.Worker;

public class TvShowDbInitializer : BackgroundService
{
    private readonly ILogger<TvShowDbInitializer> _logger;
    private readonly IServiceProvider _serviceProvider;

    public TvShowDbInitializer(IServiceProvider serviceProvider, ILogger<TvShowDbInitializer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting db initialization work...");
        await DoWorkAsync();
        _logger.LogInformation("Db initialization job finished.");
    }

    private async Task DoWorkAsync()
    {
        try
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TvShowTrackerDbContext>();
            await dbContext.Database.MigrateAsync();
            await SeedRoles(dbContext);
            await SeedUsers(dbContext);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred during db initialization, details: {details}", ex.ToString());
        }
    }

    private async Task SeedUsers(TvShowTrackerDbContext context)
    {
        if (context.Users.Any(u => u.Email == "admin@admin.com")) return;
        var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Administrator");
        if (adminRole is null) return;

        var admin = new User 
        { 
            Email = "admin@admin.com", 
            FirstName = "Admin",
            LastName = "Admin", 
            Role = adminRole,
            Password = "$2a$11$yvCVm7WPgX/sTNrU/ZWDQ.qdLlUkzynvzyqXWAb0jywjUVW8QXNCq" //adminSecretPassword
        };
        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }

    private async Task SeedRoles(TvShowTrackerDbContext context)
    {
        if (context.Roles.Any()) return;
        var roleList = new List<string> { "User", "Administrator" };
        context.AddRange(roleList.Select(role =>
                                             new Role
                                             {
                                                 Name = role
                                             }));
        await context.SaveChangesAsync();
    }
}