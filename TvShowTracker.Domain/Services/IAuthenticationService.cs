using TvShowTracker.Domain.Models;

namespace TvShowTracker.Domain.Services
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Authenticates the user based on the provided credentials
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns>The authentication result depending whether the credentials are valid or not</returns>
        public Task<AuthenticationResult> AuthenticateAsync(LoginModel credentials);
    }
}
