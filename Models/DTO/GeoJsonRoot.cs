using System.Text.Json.Serialization;

namespace HackYeah_Backend.Models.DTO
{
    public class GeoJsonRoot
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("features")]
        public List<GeoFeatureDto>? Features { get; set; }
    }
}
