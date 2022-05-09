using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvShowTracker.Domain.Models;

namespace TvShowTracker.Domain.Services
{
    public interface IAuthenticationService
    {
        public Task<bool> CheckCredentialsAsync(UserCredentialsDto login);
        public Task<Result<UserDto>> AuthenticateAsync(UserCredentialsDto login);
    }
}
