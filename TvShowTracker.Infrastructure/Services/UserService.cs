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
using TvShowTracker.DataAccessLayer;
using TvShowTracker.DataAccessLayer.Models;
using TvShowTracker.Domain.Models;
using TvShowTracker.Domain.Services;
using TvShowTracker.Infrastructure.Helpers;

namespace TvShowTracker.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly TvShowTrackerDbContext _context;
        private readonly IValidator<UserDto> _validator;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        
        public UserService(TvShowTrackerDbContext context,IValidator<UserDto> validator, IMapper mapper, ILogger<UserService> logger)
        {
            _context = context;
            _validator = validator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<UserDto>> CreateAsync(UserDto user)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(user);
                if (!validationResult.IsValid)
                {
                    return ResultHelper.ToErrorResult<UserDto>(validationResult);
                }

                var dbUser = _mapper.Map<User>(user);
                _context.Add(dbUser);

                await _context.SaveChangesAsync();
                return ResultHelper.ToSuccessResult(_mapper.Map<UserDto>(dbUser));
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
                if (user.Id == null)
                {
                    return ResultHelper.ToErrorResult<UserDto>(new List<string>() { "User not found." });
                }

                var validationResult = await _validator.ValidateAsync(user);

                if (!validationResult.IsValid)
                {
                    return ResultHelper.ToErrorResult<UserDto>(validationResult);
                }

                var dbUser = _mapper.Map<User>(user);
                if (!_context.Users.Any(u => u.Id == dbUser.Id))
                {
                    ResultHelper.ToErrorResult<UserDto>(new List<string>() { "User not found." });
                }

                _context.Users.Update(dbUser);
                await _context.SaveChangesAsync();

                return ResultHelper.ToSuccessResult(_mapper.Map<UserDto>(dbUser));
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
            return await _context.Users.FirstOrDefaultAsync(u => u.IsActive && u.Id == id);
        }
        public async Task<Result<bool>> DeactivateAsync(int id)
        {
            try
            {
                var user = await GetByIdInternalAsync(id);
                if (user == null)
                {
                    ResultHelper.ToErrorResult<bool>(new List<string>() { "User not found." });
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

        public async Task<Result<IEnumerable<UserDto>>> GetAllAsync()
        {
            try
            {
                var users = await _context.Users.Where(u => u.IsActive).ToListAsync();
                var userDtos = users.Select(u => _mapper.Map<UserDto>(u));
                return ResultHelper.ToSuccessResult(userDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return ResultHelper.ToErrorResult<IEnumerable<UserDto>>(new List<string>() { ex.ToString() });
            }
        }
    }
}
