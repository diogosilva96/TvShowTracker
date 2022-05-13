using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvShowTracker.Domain.Models
{
    public class EpisodeModel
    {
        public string? Id { get; set; }

        public string Name { get; set; }

        public int Season { get; set; }

        public int Number { get; set; }

        public DateTime? AddedAt { get; set; }

        public DateTime? AirDate { get; set; }

        public TvShowDetailsModel Show { get; set; }
        public bool? IsActive { get; set; }
    }
}
