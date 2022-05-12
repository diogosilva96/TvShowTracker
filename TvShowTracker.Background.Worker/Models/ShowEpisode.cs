using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TvShowTracker.Background.Worker.Models
{
    public class ShowEpisode
    {
        [JsonPropertyName("season")] 
        public int? Season { get; set; }

        [JsonPropertyName("episode")] 
        public int? Episode { get; set; }

        [JsonPropertyName("name")] 
        public string? Name { get; set; }

        [JsonPropertyName("air_date")] 
        public string? AirDate { get; set; }
    }
}
