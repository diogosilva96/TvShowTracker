using Microsoft.EntityFrameworkCore;
using TvShowTracker.Background.Worker;
using TvShowTracker.DataAccessLayer;
using TvShowTracker.Infrastructure.Extensions;

var host = Host.CreateDefaultBuilder(args)
               .ConfigureSerilog()
               .ConfigureServices((hostContext,services) =>
               {
                   services.AddDbContext<TvShowTrackerDbContext>(opts => opts.UseSqlServer(hostContext.Configuration.GetConnectionString("TvShowTrackerDb")));
                   services.AddHostedService<TvShowTrackerWorker>();
                   services.AddHostedService<TvShowDbInitializer>();
               })
               .Build();

await host.RunAsync();
