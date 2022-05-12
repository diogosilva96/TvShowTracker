using System.Text.Json.Serialization;

namespace TvShowTracker.Background.Worker.Models
{
    public class TvShowPageResult
    {
        [JsonPropertyName("total")]
        public string Total { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("pages")]
        public int Pages { get; set; }

        [JsonPropertyName("tv_shows")]
        public List<Show> Shows { get; set; }
    }

    public class Show
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("permalink")]
        public string Permalink { get; set; }

        [JsonPropertyName("start_date")]
        public string StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public object EndDate { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("network")]
        public string Network { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("image_thumbnail_path")]
        public string ImageThumbnailPath { get; set; }
    }

}
