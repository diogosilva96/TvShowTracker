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

        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? Country { get; set; }

        public string? Network { get; set; }

        public string? Status { get; set; }

        public IEnumerable<EpisodeModel> Episodes { get; set; }

        public ICollection<ActorModel> Cast { get; set; }

        public IEnumerable<GenreModel> Genres { get; set; }

        public IEnumerable<UserModel> FavoriteUsers { get; set; }

        public bool IsActive { get; set; }
    }
}
