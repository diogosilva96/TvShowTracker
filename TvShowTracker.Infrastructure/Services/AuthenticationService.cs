using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TvShowTracker.DataAccessLayer;
using TvShowTracker.Domain.Models;
using TvShowTracker.Domain.Services;
using TvShowTracker.Infrastructure.Helpers;

namespace TvShowTracker.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly TvShowTrackerDbContext _context;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(TvShowTrackerDbContext context, ILogger<AuthenticationService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Result<bool>> CheckCredentialsAsync(AuthenticationRequest credentials)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == credentials.Email.ToLower());
                if (user == null)
                {
                    return ResultHelper.ToSuccessResult(false);
                }

                var passwordMatches = BCrypt.Net.BCrypt.Verify(credentials.Password, user.Password);
                return ResultHelper.ToSuccessResult(passwordMatches);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while validating credentials for user {email}. Error details: {details}", credentials.Email, ex.ToString());
                return ResultHelper.ToErrorResult<bool>(new List<string>() { "Error occurred while checking credentials." });
            }
        }

        public Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest credentials)
        {
            throw new NotImplementedException();
        }
    }
}
