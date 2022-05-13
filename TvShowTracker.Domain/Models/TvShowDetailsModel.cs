using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvShowTracker.Domain.Models
{
    public class TvShowDetailsModel : TvShowModel
    {
        public string? Description { get; set; }
        public IEnumerable<EpisodeModel> Episodes { get; set; }

        public ICollection<ActorModel> Cast { get; set; }

        public IEnumerable<GenreModel> Genres { get; set; }

        public IEnumerable<UserModel> FavoriteUsers { get; set; }

        public bool IsActive { get; set; }
    }
}
