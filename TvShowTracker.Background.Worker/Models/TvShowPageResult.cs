using System.Text.Json.Serialization;

namespace TvShowTracker.Background.Worker.Models
{
    public class TvShowPageResult
    {
        [JsonPropertyName("total")]
        public string? Total { get; set; }

        [JsonPropertyName("page")]
        public int? Page { get; set; }

        [JsonPropertyName("pages")]
        public int? Pages { get; set; }

        [JsonPropertyName("tv_shows")]
        public List<Show>? Shows { get; set; }
    }
}
