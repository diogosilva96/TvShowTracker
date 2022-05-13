using System.Diagnostics;
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
            Password = BCrypt.Net.BCrypt.HashPassword("superSecretPassword") //adminSecretPassword
        };
        context.Users.Add(admin);
        var random = new Random();
        await context.SaveChangesAsync();
        var firstNames = new []
        {
            "Harry", "Ross",
            "Bruce", "Cook",
            "Carolyn", "Morgan",
            "Albert", "Walker",
            "Randy", "Reed",
            "Larry", "Barnes",
            "Lois", "Wilson",
            "Jesse", "Campbell",
            "Ernest", "Rogers",
            "Theresa", "Patterson",
            "Henry", "Simmons",
            "Michelle", "Perry",
            "Frank", "Butler",
            "Shirley"
        };
        var lastNames = new []
        {
            "Ruth", "Jackson",
            "Debra", "Allen",
            "Gerald", "Harris",
            "Raymond", "Carter",
            "Jacqueline", "Torres",
            "Joseph", "Nelson",
            "Carlos", "Sanchez",
            "Ralph", "Clark",
            "Jean", "Alexander",
            "Stephen", "Roberts",
            "Eric", "Long",
            "Amanda", "Scott",
            "Teresa", "Diaz",
            "Wanda", "Thomas"
        };
        var users = new List<User>();
        var userRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
        if (userRole is null) return;
        //create random users
        for (int i = 0; i < 100; i++)
        {
                var firstName = string.Empty;
                var lastName = string.Empty;
                do
                {
                    firstName = firstNames[random.Next(0, firstNames.Length)];
                    lastName = lastNames[random.Next(0, lastNames.Length)];

                } while (users.Any(u => u.FirstName == firstName && u.LastName == lastName));

                var email = $"{firstName}.{lastName}@email.com";

                var user = new User()
                {
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    Password = BCrypt.Net.BCrypt.HashPassword($"pw{firstName}{lastName}".ToLower()),
                    Role = userRole
                };
                users.Add(user);
        }
        context.Users.AddRange(users);
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