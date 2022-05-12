using Microsoft.EntityFrameworkCore;
using TvShowTracker.Background.Worker;
using TvShowTracker.Background.Worker.Extensions;
using TvShowTracker.Background.Worker.Services;
using TvShowTracker.Background.Worker.Utility;
using TvShowTracker.DataAccessLayer;
using TvShowTracker.Infrastructure.Extensions;

var host = Host.CreateDefaultBuilder(args)
               .ConfigureSerilog()
               .ConfigureServices((hostContext,services) =>
               {
                   //services.AddSingleton<IEpisodeDateApiService, EpisodeDateApiService>();
                   //services.AddHttpClient<IEpisodeDateApiService, EpisodeDateApiService>(client =>
                   //{
                   //    client.BaseAddress = new Uri(hostContext.Configuration["EpisodeDateBaseUrl"]);
                   //});

                   // Note: I wasn't able to use the AddHttpClient method above (was throwing an exception),
                   // so I figured I should just make an extension method for EpisodeDateApiService
                   services.AddEpisodeDateApiService(clientConfiguration =>
                   {
                       clientConfiguration.BaseAddress = new Uri(hostContext.Configuration["EpisodeDateBaseUrl"]);
                   });
                   services.AddDbContext<TvShowTrackerDbContext>(opts => opts.UseSqlServer(hostContext.Configuration.GetConnectionString("TvShowTrackerDb")));
                   services.AddScoped<ISynchronizationService, SynchronizationService>();
                   services.AddAutoMapper(typeof(SynchronizationMappingProfile));
                   services.AddHostedService<TvShowTrackerWorker>();
                   services.AddHostedService<TvShowDbInitializer>();
                   
               })
               .Build();

await host.RunAsync();
