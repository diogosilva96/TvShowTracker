using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TvShowTracker.Background.Worker.Models;

namespace TvShowTracker.Background.Worker.Services
{
    public class EpisodeDateApiService : IEpisodeDateApiService
    {
        private readonly HttpClient _client;
        private readonly ILogger<EpisodeDateApiService> _logger;

        public EpisodeDateApiService(HttpClient client, ILogger<EpisodeDateApiService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<TvShowPageResult?> GetMostPopularShowsAsync(int page) => await ExecuteInternalAsync<TvShowPageResult>(() => _client.GetAsync($"/api/most-popular?page={page}"));
        

        public async Task<TvShowDetailsResult?> GetTvShowDetails(string showName) => await ExecuteInternalAsync<TvShowDetailsResult>(() => _client.GetAsync($"/api/show-details?q={showName}"));

        private async Task<T?> ExecuteInternalAsync<T>(Func<Task<HttpResponseMessage>> executeMethod) where T : class
        {
            try
            {
                var response = await executeMethod();

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while fetching data for method {methodName}. Error details: {details}",executeMethod.Method.Name,ex.ToString());
            }

            return null;
        }
    }
}
