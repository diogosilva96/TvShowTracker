using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TvShowTracker.Domain.Models
{
    public class UserDto
    {
        public string? Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Password { get; set; }

        public string Email { get; set; }

        public DateTime? RegisteredAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public IEnumerable<TvShowDto> FavoriteShows { get; set; }
        public bool IsActive { get; set; }
    }
}
