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
        /// <summary>
        /// Registers a user
        /// </summary>
        /// <param name="registerUser"></param>
        /// <returns></returns>
        public Task<Result<UserModel>> RegisterAsync(RegisterUserModel registerUser);
        /// <summary>
        /// Gets all the favorite shows of a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="requesterId"></param>
        /// <returns></returns>
        public Task<Result<IEnumerable<TvShowModel>>> GetFavoriteShowsAsync(int userId, int requesterId);
        /// <summary>
        /// Adds a favorite show to a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="showIds"></param>
        /// <param name="requesterId"></param>
        /// <returns></returns>
        public Task<Result<bool>> AddFavoriteShowsAsync(int userId, IEnumerable<int> showIds, int requesterId);
        /// <summary>
        /// Removes a favorite show from a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="showIds"></param>
        /// <param name="requesterId"></param>
        /// <returns></returns>
        public Task<Result<bool>> RemoveFavoriteShowsAsync(int userId, IEnumerable<int> showIds, int requesterId);
        public Task<Result<UserModel>> UpdateAsync(UserModel target);

        /// <summary>
        /// Gets a user by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="requesterId"></param>
        /// <returns></returns>
        public Task<Result<UserModel>> GetByIdAsync(int id, int requesterId);

        /// <summary>
        /// Deactivates a user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="requesterId"></param>
        /// <returns></returns>
        public Task<Result<bool>> DeactivateAsync(int id , int requesterId);

        /// <summary>
        /// Gets all users based on provided filters
        /// </summary>
        /// <param name="requesterId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<Result<IEnumerable<UserModel>>> GetAllAsync(int requesterId, GetUsersFilter filter);

    }
}
