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
        public Task<Result<UserDto>> RegisterAsync(RegisterUserDto registerUser);
        public Task<Result<IEnumerable<TvShowDto>>> GetFavoriteShowsAsync(int userId);

        public Task<Result<bool>> AddFavoriteShowsAsync(int userId, IEnumerable<int> showIds);

        public Task<Result<bool>> RemoveFavoriteShowsAsync(int userId, IEnumerable<int> showIds);
    }
}
