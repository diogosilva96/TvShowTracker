using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvShowTracker.Domain.Models
{
    public class TvShowDto
    {
        public string? Id { get; set; }

        public string Title { get; set; }

        public string Synopsis { get; set; }

        public DateTime? AddedAt { get; set; }

        public DateTime? ReleasedAt { get; set; }

        public IEnumerable<EpisodeDto> Episodes { get; set; }

        public ICollection<ActorDto> Cast { get; set; }

        public IEnumerable<GenreDto> Genres { get; set; }

        public IEnumerable<UserDto> FavoriteUsers { get; set; }

        public bool IsActive { get; set; }
    }
}
