using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using TvShowTracker.DataAccessLayer;
using TvShowTracker.DataAccessLayer.Models;
using TvShowTracker.Domain.Models;
using TvShowTracker.Domain.Services;
using TvShowTracker.Infrastructure.Helpers;
using Role = TvShowTracker.Domain.Enums.Role;

namespace TvShowTracker.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly TvShowTrackerDbContext _context;
        private readonly IValidator<UserDto> _userValidator;
        private readonly IValidator<RegisterUserDto> _registerValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        
        public UserService(TvShowTrackerDbContext context,IValidator<UserDto> userUserValidator, IValidator<RegisterUserDto> registerValidator, IMapper mapper, ILogger<UserService> logger)
        {
            _context = context;
            _userValidator = userUserValidator;
            _registerValidator = registerValidator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterUserDto user)
        {
            try
            {
                var validationResult = await _registerValidator.ValidateAsync(user);
                if (!validationResult.IsValid)
                {
                    return ResultHelper.ToErrorResult<UserDto>(validationResult);
                }

                var userWithEmailExists =
                    await _context.Users.AnyAsync(u => u.Email.ToLower() == user.Email.ToLower());
                if (userWithEmailExists)
                {
                    return ResultHelper.ToErrorResult<UserDto>(new List<string>() { "Email is already in use." });
                }
                var dbUser = _mapper.Map<User>(user);
                // add user role by default
                var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name.ToLower() == Role.User.ToString().ToLower());
                if (userRole is not null)
                {
                    dbUser.RoleId = userRole.Id;
                }
                //dbUser.IsActive = true;
                _context.Add(dbUser);

                await _context.SaveChangesAsync();
                var userDto = _mapper.Map<UserDto>(dbUser);
                return ResultHelper.ToSuccessResult(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return ResultHelper.ToErrorResult<UserDto>(new List<string>() { ex.ToString() });
            }
        }

        public async Task<Result<UserDto>> UpdateAsync(UserDto user)
        {
            try
            {
                if (user.Id is null)
                {
                    return ResultHelper.ToErrorResult<UserDto>(new List<string>() { "User not found." });
                }
                
                var validationResult = await _userValidator.ValidateAsync(user);

                if (!validationResult.IsValid)
                {
                    return ResultHelper.ToErrorResult<UserDto>(validationResult);
                }

                var dbUser = _mapper.Map<User>(user);
                if (!_context.Users.Any(u => u.Id == dbUser.Id))
                {
                    return ResultHelper.ToErrorResult<UserDto>(new List<string>() { "User not found." });
                }

                if (!_context.Users.Any(u => u.Id == dbUser.Id && u.Email.Equals(user.Email, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return ResultHelper.ToErrorResult<UserDto>(new List<string>() { "Email cannot be changed." });
                }

                _context.Users.Update(dbUser);
                await _context.SaveChangesAsync();

                return ResultHelper.ToSuccessResult<UserDto>(_mapper.Map<UserDto>(dbUser));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return ResultHelper.ToErrorResult<UserDto>(new List<string>() { ex.ToString() });
            }
        }

        public async Task<Result<UserDto>> GetByIdAsync(int id)
        {
            var user = await GetByIdInternalAsync(id);
            return ResultHelper.ToSuccessResult(_mapper.Map<UserDto>(user));
        }

        private async Task<User?> GetByIdInternalAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<Result<bool>> DeactivateAsync(int id)
        {
            try
            {
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
                _logger.LogError(ex.ToString());
                return ResultHelper.ToErrorResult<bool>(new List<string>() { ex.ToString() });
            }
        }

        public async Task<Result<IEnumerable<UserDto>>> GetAllAsync(int? page = null, int? size = null)
        {
            try
            {
                List<User>? users = null;
                if (size is null && page is not null)
                {
                    return ResultHelper.ToErrorResult<IEnumerable<UserDto>>(new List<string>() { "Size should not be null when page is not null." });
                }

                if (page is null && size is null)
                {
                    users = await _context.Users.OrderBy(u => u.Id).ToListAsync();
                }

                if (size is not null && page is not null)
                {
                    users = await _context.Users.OrderBy(u=> u.Id).Skip(page.Value*size.Value).Take(size.Value).ToListAsync();
                }
                
                var userDtos = users is not null ? users.Select(u => _mapper.Map<UserDto>(u)) : Enumerable.Empty<UserDto>();
                return ResultHelper.ToSuccessResult(userDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return ResultHelper.ToErrorResult<IEnumerable<UserDto>>(new List<string>() { ex.ToString() });
            }
        }

        public async Task<Result<IEnumerable<TvShowDto>>> GetFavoriteShowsAsync(int id)
        {
            try
            {
                var user = await GetByIdInternalAsync(id);
                if (user is null)
                {
                    ResultHelper.ToSuccessResult(Enumerable.Empty<TvShowDto>());
                }

                var shows = user.FavoriteShows.Select(s => _mapper.Map<TvShowDto>(s));
                return ResultHelper.ToSuccessResult(shows);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return ResultHelper.ToErrorResult<IEnumerable<TvShowDto>>(new List<string>() { ex.ToString() });
            }
        }

        public async Task<Result<bool>> AddFavoriteShowsAsync(int userId, IEnumerable<int> showIds)
        {
            try
            {
                var user = await GetByIdInternalAsync(userId);
                if (user is null)
                {
                   return ResultHelper.ToSuccessResult(false);
                }

                if (user.FavoriteShows.All(s => showIds.Contains(s.Id)))
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
                _logger.LogError(ex.ToString());
                return ResultHelper.ToErrorResult<bool>(new List<string>(){ ex.ToString() });
            }
        }

        public async Task<Result<bool>> RemoveFavoriteShowsAsync(int userId, IEnumerable<int> showIds)
        {
            try
            {
                var user = await GetByIdInternalAsync(userId);

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
                _logger.LogError(ex.ToString());
                return ResultHelper.ToErrorResult<bool>(new List<string>() { ex.ToString() });
            }
        }
    }
}
