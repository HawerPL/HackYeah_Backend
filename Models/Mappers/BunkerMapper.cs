using HackYeah_Backend.Models.DTO;
using HackYeah_Backend.Utilits;

namespace HackYeah_Backend.Models.Mappers
{
    public static class BunkerMapper
    {
        public static Bunker ToEntity(GeoFeatureDto feature)
        {
            int id = 0;
            if (feature.Id != null)
            {
                var parts = feature.Id.Split('/');
                if (parts.Length == 2)
                    int.TryParse(parts[1], out id);
            }

            float x = 0, y = 0;
            if (feature.Geometry?.Coordinates?.Count >= 2)
            {
                x = float.TryParse(feature.Geometry.Coordinates[0]?.ToString(), out var xx) ? xx : 0;
                y = float.TryParse(feature.Geometry.Coordinates[1]?.ToString(), out var yy) ? yy : 0;
            }


            object? name = null;
            object? building = null;
            if (feature.Properties != null)
            {
                feature.Properties.TryGetValue("name", out name);
                feature.Properties.TryGetValue("building", out building);
            }

            return new Bunker
            {
                Id = id,
                x = x,
                y = y,
                Name = Utils.TryParseStr(name),
                Building = Utils.TryParseStr(building),
            };
        }
    }
}
