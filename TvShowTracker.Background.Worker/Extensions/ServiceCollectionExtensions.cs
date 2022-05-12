using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvShowTracker.Background.Worker.Services;

namespace TvShowTracker.Background.Worker.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEpisodeDateApiService(this IServiceCollection self, Action<HttpClient> clientConfiguration)
        {
            self.AddSingleton<IEpisodeDateApiService>(provider =>
            {
                var httpClient = new HttpClient();
                clientConfiguration(httpClient);
                return new EpisodeDateApiService(httpClient, provider.GetRequiredService<ILogger<EpisodeDateApiService>>());
            });
        }
    }
}
