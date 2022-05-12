using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvShowTracker.Domain.Models
{
    public class CredentialCheckResult
    {
        public bool IsValid { get; set; }

        public int? UserId { get; set; }

        public string? Email { get; set; }

        public IEnumerable<string>? Roles { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
