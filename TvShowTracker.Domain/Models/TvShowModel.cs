using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvShowTracker.Domain.Models
{
    public class TvShowModel
    {
        public string? Id { get; set; }

        public string Title { get; set; }

        public string Synopsis { get; set; }

        public DateTime? AddedAt { get; set; }

        public DateTime? ReleasedAt { get; set; }

        public IEnumerable<EpisodeModel> Episodes { get; set; }

        public ICollection<ActorModel> Cast { get; set; }

        public IEnumerable<GenreModel> Genres { get; set; }

        public IEnumerable<UserModel> FavoriteUsers { get; set; }

        public bool IsActive { get; set; }
    }
}
