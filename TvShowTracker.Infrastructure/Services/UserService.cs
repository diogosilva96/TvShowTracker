using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TvShowTracker.DataAccessLayer;
using TvShowTracker.DataAccessLayer.Models;
using TvShowTracker.Domain.Models;
using TvShowTracker.Domain.Services;
using TvShowTracker.Infrastructure.Helpers;
using TvShowTracker.Infrastructure.Utilities;
using TvShowTracker.Infrastructure.Validators;


namespace TvShowTracker.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly TvShowTrackerDbContext _context;
        private readonly IValidator<UserModel> _userValidator;
        private readonly IValidator<RegisterUserModel> _registerValidator;
        private readonly IValidator<PagedFilter> _pagedFilterValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        
        public UserService(TvShowTrackerDbContext context, IValidator<UserModel> userValidator, IValidator<RegisterUserModel> registerValidator, IValidator<PagedFilter> pagedFilterValidator, IMapper mapper, ILogger<UserService> logger)
        {
            _context = context;
            _userValidator = userValidator;
            _registerValidator = registerValidator;
            _pagedFilterValidator = pagedFilterValidator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<UserModel>> RegisterAsync(RegisterUserModel user)
        {
            try
            {
                var validationResult = await _registerValidator.ValidateAsync(user);
                if (!validationResult.IsValid)
                {
                    return ResultHelper.ToErrorResult<UserModel>(validationResult);
                }

                var userWithEmailExists =
                    await _context.Users.AnyAsync(u => u.Email.ToLower() == user.Email.ToLower());
                if (userWithEmailExists)
                {
                    return ResultHelper.ToErrorResult<UserModel>(new List<string>() { "Email is already in use." });
                }
                var dbUser = _mapper.Map<User>(user);
                // add user role by default
                var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name.ToLower() == UserRoles.User);
                if (userRole is not null)
                {
                    dbUser.RoleId = userRole.Id;
                }
                _context.Add(dbUser);

                await _context.SaveChangesAsync();
                var userModel = _mapper.Map<UserModel>(dbUser);
                return ResultHelper.ToSuccessResult(userModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred on {methodName}. Error details {details}", nameof(RegisterAsync), ex.ToString());
                return ResultHelper.ToErrorResult<UserModel>(new List<string>() { ex.ToString() });
            }
        }

        public async Task<Result<UserModel>> UpdateAsync(UserModel user)
        {
            try
            {
                if (user.Id is null)
                {
                    return ResultHelper.ToErrorResult<UserModel>(new List<string>() { "User not found." });
                }
                
                var validationResult = await _userValidator.ValidateAsync(user);

                if (!validationResult.IsValid)
                {
                    return ResultHelper.ToErrorResult<UserModel>(validationResult);
                }

                var dbUser = _mapper.Map<User>(user);
                if (!_context.Users.Any(u => u.Id == dbUser.Id))
                {
                    return ResultHelper.ToErrorResult<UserModel>(new List<string>() { "User not found." });
                }

                if (!_context.Users.Any(u => u.Id == dbUser.Id && u.Email.Equals(user.Email, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return ResultHelper.ToErrorResult<UserModel>(new List<string>() { "Email cannot be changed." });
                }

                _context.Users.Update(dbUser);
                await _context.SaveChangesAsync();

                return ResultHelper.ToSuccessResult<UserModel>(_mapper.Map<UserModel>(dbUser));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred on {methodName}. Error details {details}", nameof(UpdateAsync), ex.ToString());
                return ResultHelper.ToErrorResult<UserModel>(new List<string>() { ex.ToString() });
            }
        }

        public async Task<Result<UserModel>> GetByIdAsync(int id, int requesterId)
        {
            if (!await IsSelfOrBelongsToRole(id, requesterId, UserRoles.Administrator))
            {
                return ResultHelper.ToErrorResult<UserModel>(new List<string>() { "Unauthorized." });
            }
            var user = await GetByIdInternalAsync(id);
            return ResultHelper.ToSuccessResult(_mapper.Map<UserModel>(user));

        }

        private async Task<User?> GetByIdInternalAsync(int id) => await _context.Users.SingleOrDefaultAsync(u => u.Id == id);

        private async Task<string?> GetRoleNameById(int userId) => (await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == userId))?.Role?.Name;


        private async Task<bool> IsSelfOrBelongsToRole(int userId, int requesterId, string role) => userId == requesterId || await IsInRole(requesterId, role);
        public async Task<Result<bool>> DeactivateAsync(int id, int requesterId)
        {
            try
            {
                if (!await IsSelfOrBelongsToRole(id,requesterId,UserRoles.Administrator))
                {
                    return ResultHelper.ToErrorResult<bool>(new List<string>() { "Unauthorized." });
                }
                var user = await GetByIdInternalAsync(id);
                if (user is null)
                {
                    return ResultHelper.ToErrorResult<bool>(new List<string>() { "User not found." });
                }

                user.IsActive = false;
                await _context.SaveChangesAsync();
                return ResultHelper.ToSuccessResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred on {methodName}. Error details {details}", nameof(DeactivateAsync), ex.ToString());
                return ResultHelper.ToErrorResult<bool>(new List<string>() { ex.ToString() });
            }
        }

        private async Task<bool> IsInRole(int userId, string role)
        {
            var userRole = await GetRoleNameById(userId);
            return userRole is not null && userRole.Equals(role, StringComparison.InvariantCultureIgnoreCase);
        }
        public async Task<Result<IEnumerable<UserModel>>> GetAllAsync(int requesterId, GetUsersFilter filter)
        {
            try
            {
                if (await IsInRole(requesterId,UserRoles.User))
                {
                    // this should never happen as long as the api is configured correctly
                    return ResultHelper.ToErrorResult<IEnumerable<UserModel>>( new List<string>{ "Not enough permissions" });
                }

                var filterValidation = await _pagedFilterValidator.ValidateAsync(filter);
                if (!filterValidation.IsValid)
                {
                    return ResultHelper.ToErrorResult<IEnumerable<UserModel>> (filterValidation);
                }
                List<User>? users = null;
                Expression<Func<User, bool>> filterExpression = user =>
                    (string.IsNullOrEmpty(filter.LastName) || user.LastName.ToLower() == filter.LastName.ToLower()) &&
                    (string.IsNullOrEmpty(filter.FirstName) || user.FirstName.ToLower() == filter.FirstName) &&
                    (string.IsNullOrEmpty(filter.Email) || user.Email.ToLower() == filter.Email.ToLower()) &&
                    (!filter.IsActive.HasValue || user.IsActive == filter.IsActive);

                if (filter.PageSize is null && filter.Page is null)
                {
                    users = await _context.Users.Where(filterExpression)
                                                .OrderBy(u => u.Id)
                                                .ToListAsync();
                }

                if (filter.PageSize is not null && filter.Page is not null)
                {
                    users = await _context.Users.Where(filterExpression)
                                                .OrderBy(u=> u.Id)
                                                .Skip(filter.Page.Value*filter.PageSize.Value)
                                                .Take(filter.PageSize.Value)
                                                .ToListAsync();
                }
                
                var userModels = users is not null && users.Any() ? 
                    users.Select(u =>
                    {
                        var model = _mapper.Map<UserModel>(u);
                        model.FavoriteShows = null;
                        return model;
                    }) : Enumerable.Empty<UserModel>();
                return ResultHelper.ToSuccessResult(userModels);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred on {methodName}. Error details {details}",nameof(GetAllAsync), ex.ToString());
                return ResultHelper.ToErrorResult<IEnumerable<UserModel>>(new List<string>() { ex.ToString() });
            }
        }

        public async Task<Result<IEnumerable<TvShowModel>>> GetFavoriteShowsAsync(int id, int requesterId)
        {
            try
            {
                if (!await IsSelfOrBelongsToRole(id, requesterId, UserRoles.Administrator))
                {
                    return ResultHelper.ToErrorResult<IEnumerable<TvShowModel>>(new List<string>() {"Unauthorized"});
                }
                var user = await _context.Users
                                         .Include(u=>u.FavoriteShows)
                                         .SingleOrDefaultAsync(u=> u.Id == id);
                if (user is null)
                {
                    return ResultHelper.ToSuccessResult(Enumerable.Empty<TvShowModel>());
                }

                var shows = user.FavoriteShows.Select(s => _mapper.Map<TvShowModel>(s));
                return ResultHelper.ToSuccessResult(shows);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred on {methodName}. Error details {details}", nameof(GetFavoriteShowsAsync), ex.ToString());
                return ResultHelper.ToErrorResult<IEnumerable<TvShowModel>>(new List<string>() { ex.ToString() });
            }
        }



        public async Task<Result<bool>> AddFavoriteShowsAsync(int userId, IEnumerable<int> showIds, int requesterId)
        {
            try
            {
                if (requesterId != userId)
                {
                    // only the self user can add favorites.
                    return ResultHelper.ToErrorResult<bool>(new List<string>() { "Unauthorized." });
                }

                var user = await _context.Users.Include(u=>u.FavoriteShows).SingleOrDefaultAsync(u => u.Id == userId);
                if (user is null)
                {
                   return ResultHelper.ToSuccessResult(false);
                }

                if (user.FavoriteShows.Any() && user.FavoriteShows.All(s => showIds.Contains(s.Id)))
                {
                    return ResultHelper.ToSuccessResult(true);
                }

                var showsToAdd = await _context.Shows
                                                           .Where(s => showIds.Contains(s.Id) && s.FavoriteUsers.All(u => u.Id != userId))
                                                           .ToListAsync();
                if (!showsToAdd.Any())
                {
                    return ResultHelper.ToSuccessResult(true);
                }
                showsToAdd.ForEach(s => user.FavoriteShows.Add(s));
                await _context.SaveChangesAsync();
                return ResultHelper.ToSuccessResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred on {methodName}. Error details {details}", nameof(AddFavoriteShowsAsync), ex.ToString());
                return ResultHelper.ToErrorResult<bool>(new List<string>(){ ex.ToString() });
            }
        }

        public async Task<Result<bool>> RemoveFavoriteShowsAsync(int userId, IEnumerable<int> showIds, int requesterId)
        {
            try
            {
                if (requesterId != userId)
                {
                    // only the self user can remove favorites.
                    return ResultHelper.ToErrorResult<bool>(new List<string>() { "Unauthorized." });
                }

                var user = await _context.Users.Include(u => u.FavoriteShows).SingleOrDefaultAsync(u => u.Id == userId);

                if (user is null)
                {
                    return ResultHelper.ToSuccessResult(false);
                }

                var showsToRemove = user.FavoriteShows.Where(s => showIds.Contains(s.Id)).ToList();
                if (!showsToRemove.Any())
                {
                    return ResultHelper.ToSuccessResult(true);
                }

                showsToRemove.ForEach(s => user.FavoriteShows.Remove(s));
                await _context.SaveChangesAsync();
                return ResultHelper.ToSuccessResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred on {methodName}. Error details {details}", nameof(RemoveFavoriteShowsAsync), ex.ToString());
                return ResultHelper.ToErrorResult<bool>(new List<string>() { ex.ToString() });
            }
        }
    }
}
