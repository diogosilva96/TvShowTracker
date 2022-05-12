using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvShowTracker.Domain.Models;

namespace TvShowTracker.Domain.Services
{
    public interface IUserService
    {
        public Task<Result<UserModel>> RegisterAsync(RegisterUserModel registerUser);
        public Task<Result<IEnumerable<TvShowModel>>> GetFavoriteShowsAsync(int userId, int requesterId);

        public Task<Result<bool>> AddFavoriteShowsAsync(int userId, IEnumerable<int> showIds);

        public Task<Result<bool>> RemoveFavoriteShowsAsync(int userId, IEnumerable<int> showIds);
        public Task<Result<UserModel>> UpdateAsync(UserModel target);

        public Task<Result<UserModel>> GetByIdAsync(int id, int requesterId);

        public Task<Result<bool>> DeactivateAsync(int id , int requesterId);

        Task<Result<IEnumerable<UserModel>>> GetAllAsync(int requesterId, GetUsersFilter filter);

    }
}
