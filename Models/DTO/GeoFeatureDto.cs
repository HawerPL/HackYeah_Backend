using System.Text.Json.Serialization;

namespace HackYeah_Backend.Models.DTO
{
    public class GeoFeatureDto
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("properties")]
        public Dictionary<string, object>? Properties { get; set; }

        [JsonPropertyName("geometry")]
        public GeometryDto? Geometry { get; set; }
    }
}
