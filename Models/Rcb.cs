using System.Text.Json.Serialization;

namespace HackYeah_Backend.Models
{
    public class Rcb
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string?  Description { get; set; }
        public DateTime EventDate { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<LocationRcb> Locations { get; set; } = new List<LocationRcb>();
        public ICollection<TagRcb> Tags { get; set; } = new List<TagRcb>();
    }

    public class LocationRcb
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public int RcbId { get; set; }
        [JsonIgnore]
        public Rcb Rcb { get; set; }
    }

    public class TagRcb
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public int RcbId { get; set; }
        [JsonIgnore]
        public Rcb Rcb { get; set; }
    }
}
