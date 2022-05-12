// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);

using System.Text.Json.Serialization;

namespace TvShowTracker.Background.Worker.Models;

public class TvShowDetailsResult
{
    [JsonPropertyName("tvShow")] public TvShow TvShow { get; set; }
}

public class TvShow
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("permalink")] public string Permalink { get; set; }

    [JsonPropertyName("url")] public string Url { get; set; }

    [JsonPropertyName("description")] public string Description { get; set; }

    [JsonPropertyName("description_source")]
    public string DescriptionSource { get; set; }

    [JsonPropertyName("start_date")] public string StartDate { get; set; }

    [JsonPropertyName("end_date")] public object EndDate { get; set; }

    [JsonPropertyName("country")] public string Country { get; set; }

    [JsonPropertyName("status")] public string Status { get; set; }

    [JsonPropertyName("runtime")] public int Runtime { get; set; }

    [JsonPropertyName("network")] public string Network { get; set; }

    [JsonPropertyName("youtube_link")] public object YoutubeLink { get; set; }

    [JsonPropertyName("image_path")] public string ImagePath { get; set; }

    [JsonPropertyName("image_thumbnail_path")]
    public string ImageThumbnailPath { get; set; }

    [JsonPropertyName("rating")] public string Rating { get; set; }

    [JsonPropertyName("rating_count")] public string RatingCount { get; set; }

    [JsonPropertyName("countdown")] public object Countdown { get; set; }

    [JsonPropertyName("genres")] public List<string> Genres { get; set; }

    [JsonPropertyName("pictures")] public List<string> Pictures { get; set; }

    [JsonPropertyName("episodes")] public List<ShowEpisode> Episodes { get; set; }
}

public class ShowEpisode
{
    [JsonPropertyName("season")] public int Season { get; set; }

    [JsonPropertyName("episode")] public int Episode { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("air_date")] public string AirDate { get; set; }
}