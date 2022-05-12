// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);

using System.Text.Json.Serialization;

namespace TvShowTracker.Background.Worker.Models;

public class TvShowDetailsResult
{
    [JsonPropertyName("tvShow")] public Show? Show { get; set; }
}


