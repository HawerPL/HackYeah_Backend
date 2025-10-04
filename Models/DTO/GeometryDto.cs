using System.Text.Json.Serialization;

namespace HackYeah_Backend.Models.DTO
{
    public class GeometryDto
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        //Point — [lon, lat]
        [JsonPropertyName("coordinates")]
        public List<object>? Coordinates { get; set; }
    }
}
