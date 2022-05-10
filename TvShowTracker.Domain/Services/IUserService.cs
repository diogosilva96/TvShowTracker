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
        public Task<IEnumerable<TvShowDto>> GetFavoriteShowsAsync(int userId);

        public Task<bool> AddFavoriteShowAsync(int showId);

        public Task<bool> RemoveFavoriteShowAsync(int showId);
    }
}
