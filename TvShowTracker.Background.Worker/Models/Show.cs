using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TvShowTracker.Background.Worker.Models
{
    public class Show
    {
        [JsonPropertyName("id")] public int? Id { get; set; }

        [JsonPropertyName("name")] public string? Name { get; set; }

        [JsonPropertyName("permalink")] public string? Permalink { get; set; }

        [JsonPropertyName("url")] public string? Url { get; set; }

        [JsonPropertyName("description")] public string? Description { get; set; }

        [JsonPropertyName("description_source")]
        public string? DescriptionSource { get; set; }

        [JsonPropertyName("start_date")] public string? StartDate { get; set; }

        [JsonPropertyName("end_date")] public string? EndDate { get; set; }

        [JsonPropertyName("country")] public string? Country { get; set; }

        [JsonPropertyName("status")] public string? Status { get; set; }

        [JsonPropertyName("runtime")] public int? Runtime { get; set; }

        [JsonPropertyName("network")] public string? Network { get; set; }

        [JsonPropertyName("youtube_link")] public string? YoutubeLink { get; set; }

        [JsonPropertyName("image_path")] public string? ImagePath { get; set; }

        [JsonPropertyName("image_thumbnail_path")]
        public string ImageThumbnailPath { get; set; }

        [JsonPropertyName("rating")] public string? Rating { get; set; }

        [JsonPropertyName("rating_count")] public string? RatingCount { get; set; }

        [JsonPropertyName("countdown")] public object? Countdown { get; set; }

        [JsonPropertyName("genres")] public List<string>? Genres { get; set; }

        [JsonPropertyName("pictures")] public List<string>? Pictures { get; set; }

        [JsonPropertyName("episodes")] public List<ShowEpisode>? Episodes { get; set; }
    }
}
