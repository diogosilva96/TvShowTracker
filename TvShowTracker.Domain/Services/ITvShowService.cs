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
        public Task<Result<IEnumerable<TvShowModel>>> GetAllAsync(GetTvShowsFilter filter);
    }
}
