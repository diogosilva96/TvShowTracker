using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvShowTracker.Domain.Models;

namespace TvShowTracker.Domain.Services
{
    public interface ITvShowService
    {
        /// <summary>
        /// Gets all tv shows based on provided filters
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task<Result<IEnumerable<TvShowModel>>> GetAllAsync(GetTvShowsFilter filter);

        /// <summary>
        /// Get the details of tv show
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Result<TvShowDetailsModel>> GetByIdAsync(int id);
    }
}
