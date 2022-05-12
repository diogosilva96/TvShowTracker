using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvShowTracker.Domain.Models
{
    public class AuthenticationResult
    {
        public string? Token { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool AuthenticationSuccess { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
