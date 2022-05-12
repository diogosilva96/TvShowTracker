using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TvShowTracker.DataAccessLayer;
using TvShowTracker.DataAccessLayer.Models;
using TvShowTracker.Domain.Models;
using TvShowTracker.Domain.Services;
using TvShowTracker.Infrastructure.Helpers;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace TvShowTracker.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly TvShowTrackerDbContext _context;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(TvShowTrackerDbContext context, JwtConfiguration jwtConfiguration, ILogger<AuthenticationService> logger)
        {
            _context = context;
            _jwtConfiguration = jwtConfiguration;
            _logger = logger;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(LoginModel credentials)
        {
            try
            {
                var user = await _context.Users.Include(u=>u.Role).FirstOrDefaultAsync(u => u.Email.ToLower() == credentials.Email.ToLower());
                if (user == null)
                {
                    return getAuthenticationFailResult();
                }

                var passwordMatches = BCrypt.Net.BCrypt.Verify(credentials.Password, user.Password);
                if (!passwordMatches)
                {
                    return getAuthenticationFailResult();
                }

                var token = GenerateToken(user);
                return new ()
                {
                    AuthenticationSuccess = true, ExpiresAt = token.ValidTo,
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while authentication user with email {email}. Error details: {details}", credentials.Email, ex.ToString());
            }
            return getAuthenticationFailResult();
            AuthenticationResult getAuthenticationFailResult() => new() { AuthenticationSuccess = false, ErrorMessage = "Invalid credentials." };
        }

        private JwtSecurityToken GenerateToken(User user)
        {
            var authClaims = new List<Claim>()
            {
                new(ClaimTypes.Name, user.Id.ToString()),
                new(ClaimTypes.Email,user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.Role, user.Role is null ? UserRoles.User : user.Role.Name)
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret));
            return new JwtSecurityToken(signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),
                                        claims: authClaims,
                                        issuer: _jwtConfiguration.Issuer,
                                        audience: _jwtConfiguration.Audience,
                                        expires: DateTime.Now.AddMinutes(_jwtConfiguration.TokenExpirationTimeInMinutes));
        }
    }
}
