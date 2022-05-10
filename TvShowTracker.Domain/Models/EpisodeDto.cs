using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvShowTracker.Domain.Models
{
    public class EpisodeDto
    {
        public string? Id { get; set; }

        public string Title { get; set; }

        public int Season { get; set; }

        public int Number { get; set; }

        public string Synopsis { get; set; }

        public DateTime? AddedAt { get; set; }

        public DateTime? ReleasedAt { get; set; }

        public TvShowDto Show { get; set; }

        public bool IsActive { get; set; }
    }
}
