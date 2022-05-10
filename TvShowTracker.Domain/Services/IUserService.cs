using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvShowTracker.Domain.Models;

namespace TvShowTracker.Domain.Services
{
    public interface IUserService : IGenericService<UserDto>
    {
        public Task<Result<IEnumerable<TvShowDto>>> GetFavoriteShowsAsync(int userId);

        public Task<Result<bool>> AddFavoriteShowAsync(int userId, int showId);

        public Task<Result<bool>> RemoveFavoriteShowAsync(int userId, int showId);
    }
}
