using TvShowTracker.Domain.Models;

namespace TvShowTracker.Domain.Services
{
    public interface IAuthenticationService
    {
        public Task<Result<bool>> CheckCredentialsAsync(AuthenticationRequest credentials);
        public Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest credentials);
    }
}
