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
            SeedRoles(dbContext);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred during db initialization, details: {details}", ex.ToString());
        }
    }

    private void SeedRoles(TvShowTrackerDbContext context)
    {
        if (context.Roles.Any()) return;
        var roleList = new List<string> { "User", "Administrator" };
        context.AddRange(roleList.Select(role =>
                                             new Role
                                             {
                                                 Name = role
                                             }));
    }

}