using TvShowTracker.Domain.Services;
using TvShowTracker.Infrastructure.Services;

namespace TvShowTracker.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHashingService(this IServiceCollection self, string saltKey)
        {
            var provider = self.BuildServiceProvider();
            self.AddSingleton<IHashingService>(new HashingService(saltKey, provider.GetRequiredService<ILogger<HashingService>>()));
        }
    }
}
