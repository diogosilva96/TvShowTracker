using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvShowTracker.Background.Worker.Models;

namespace TvShowTracker.Background.Worker.Services
{
    public interface IEpisodeDateApiService
    {
        /// <summary>
        /// Gets the most popular shows from the api
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<TvShowPageResult?> GetMostPopularShowsAsync(int page);

        Task<TvShowDetailsResult?> GetTvShowDetails(string showName);
    }
}
