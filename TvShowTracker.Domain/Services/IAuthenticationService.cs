using TvShowTracker.Domain.Models;

namespace TvShowTracker.Domain.Services
{
    public interface IAuthenticationService
    {
        public Task<bool> CheckCredentialsAsync(string email, string password);
        public Task<Result<UserDto>> AuthenticateAsync(string email, string password);
    }
}
