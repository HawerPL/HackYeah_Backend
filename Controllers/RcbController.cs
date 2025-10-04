using HackYeah_Backend.Data;
using HackYeah_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HackYeah_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RcbController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RcbController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Rcb
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rcb>>> GetAll()
        {
            var alerts = await _context.Rcbs
                .Include(r => r.Locations)
                .Include(r => r.Tags)
                .OrderByDescending(a => a.EventDate)
                .ToListAsync();

            return Ok(alerts);
        }

        // GET: api/Rcb/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rcb>> GetById(int id)
        {
            var alert = await _context.Rcbs
                .Include(r => r.Locations)
                .Include(r => r.Tags)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (alert == null)
            {
                return NotFound($"Alert o ID {id} nie został znaleziony.");
            }

            return Ok(alert);
        }

        // GET: api/Rcb/location/{locationName}
        [HttpGet("location/{locationName}")]
        public async Task<ActionResult<IEnumerable<Rcb>>> GetByLocation(string locationName)
        {
            var alerts = await _context.Rcbs
                .Include(r => r.Locations)
                .Include(r => r.Tags)
                .Where(r => r.Locations.Any(l =>
                    EF.Functions.Like(l.Name, $"%{locationName}%")))
                .OrderByDescending(a => a.EventDate)
                .ToListAsync();

            if (!alerts.Any())
            {
                return NotFound($"Nie znaleziono alertów dla lokalizacji: {locationName}");
            }

            return Ok(alerts);
        }

        // GET: api/Rcb/locations?locations=mazowieckie,podkarpackie
        [HttpGet("locations")]
        public async Task<ActionResult<IEnumerable<Rcb>>> GetByMultipleLocations([FromQuery] string locations)
        {
            if (string.IsNullOrWhiteSpace(locations))
            {
                return BadRequest("Należy podać przynajmniej jedną lokalizację.");
            }

            var locationList = locations.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Trim().ToLower())
                .ToList();

            var alerts = await _context.Rcbs
                .Include(r => r.Locations)
                .Include(r => r.Tags)
                .Where(r => r.Locations.Any(l =>
                    locationList.Contains(l.Name.ToLower())))
                .OrderByDescending(a => a.EventDate)
                .ToListAsync();

            return Ok(alerts);
        }

        // GET: api/Rcb/available-locations
        [HttpGet("available-locations")]
        public async Task<ActionResult<IEnumerable<string>>> GetAvailableLocations()
        {
            var locations = await _context.Set<LocationRcb>()
                .Select(l => l.Name)
                .Distinct()
                .OrderBy(l => l)
                .ToListAsync();

            return Ok(locations);
        }

        // GET: api/Rcb/tag/{tagName}
        [HttpGet("tag/{tagName}")]
        public async Task<ActionResult<IEnumerable<Rcb>>> GetByTag(string tagName)
        {
            var alerts = await _context.Rcbs
                .Include(r => r.Locations)
                .Include(r => r.Tags)
                .Where(r => r.Tags.Any(t =>
                    EF.Functions.Like(t.Name, $"%{tagName}%")))
                .OrderByDescending(a => a.EventDate)
                .ToListAsync();

            if (!alerts.Any())
            {
                return NotFound($"Nie znaleziono alertów z tagiem: {tagName}");
            }

            return Ok(alerts);
        }

        // GET: api/Rcb/tags?tags=szczepionka,woda
        [HttpGet("tags")]
        public async Task<ActionResult<IEnumerable<Rcb>>> GetByMultipleTags([FromQuery] string tags)
        {
            if (string.IsNullOrWhiteSpace(tags))
            {
                return BadRequest("Należy podać przynajmniej jeden tag.");
            }

            var tagList = tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim().ToLower())
                .ToList();

            var alerts = await _context.Rcbs
                .Include(r => r.Locations)
                .Include(r => r.Tags)
                .Where(r => r.Tags.Any(t =>
                    tagList.Contains(t.Name.ToLower())))
                .OrderByDescending(a => a.EventDate)
                .ToListAsync();

            return Ok(alerts);
        }

        // GET: api/Rcb/available-tags
        [HttpGet("available-tags")]
        public async Task<ActionResult<IEnumerable<string>>> GetAvailableTags()
        {
            var tags = await _context.Set<TagRcb>()
                .Select(t => t.Name)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();

            return Ok(tags);
        }

        // GET: api/Rcb/recent?days=7
        [HttpGet("recent")]
        public async Task<ActionResult<IEnumerable<Rcb>>> GetRecent([FromQuery] int days = 7)
        {
            var cutoffDate = DateTime.Now.AddDays(-days);

            var alerts = await _context.Rcbs
                .Include(r => r.Locations)
                .Include(r => r.Tags)
                .Where(a => a.EventDate >= cutoffDate)
                .OrderByDescending(a => a.EventDate)
                .ToListAsync();

            return Ok(alerts);
        }

        // GET: api/Rcb/search?query=szczepionka
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Rcb>>> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Zapytanie wyszukiwania nie może być puste.");
            }

            var alerts = await _context.Rcbs
                .Include(r => r.Locations)
                .Include(r => r.Tags)
                .Where(a =>
                    (a.Title != null && EF.Functions.Like(a.Title, $"%{query}%")) ||
                    (a.Description != null && EF.Functions.Like(a.Description, $"%{query}%"))
                )
                .OrderByDescending(a => a.EventDate)
                .ToListAsync();

            return Ok(alerts);
        }

        // GET: api/Rcb/filter?location=mazowieckie&tags=szczepionka,zwierzęta&days=30
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Rcb>>> GetFiltered(
            [FromQuery] string? location = null,
            [FromQuery] string? tags = null,
            [FromQuery] int? days = null)
        {
            var query = _context.Rcbs
                .Include(r => r.Locations)
                .Include(r => r.Tags)
                .AsQueryable();

            // Filtruj po lokalizacji
            if (!string.IsNullOrWhiteSpace(location))
            {
                var locationList = location.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(l => l.Trim().ToLower())
                    .ToList();

                query = query.Where(r => r.Locations.Any(l =>
                    locationList.Contains(l.Name.ToLower())));
            }

            // Filtruj po tagach
            if (!string.IsNullOrWhiteSpace(tags))
            {
                var tagList = tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => t.Trim().ToLower())
                    .ToList();

                query = query.Where(r => r.Tags.Any(t =>
                    tagList.Contains(t.Name.ToLower())));
            }

            // Filtruj po dacie
            if (days.HasValue && days.Value > 0)
            {
                var cutoffDate = DateTime.Now.AddDays(-days.Value);
                query = query.Where(a => a.EventDate >= cutoffDate);
            }

            var result = await query
                .OrderByDescending(a => a.EventDate)
                .ToListAsync();

            return Ok(result);
        }

        // GET: api/Rcb/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount()
        {
            var count = await _context.Rcbs.CountAsync();
            return Ok(count);
        }

        // GET: api/Rcb/statistics
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetStatistics()
        {
            var totalAlerts = await _context.Rcbs.CountAsync();
            var cutoffDate7 = DateTime.Now.AddDays(-7);
            var cutoffDate30 = DateTime.Now.AddDays(-30);

            var alertsLast7Days = await _context.Rcbs
                .CountAsync(a => a.EventDate >= cutoffDate7);

            var alertsLast30Days = await _context.Rcbs
                .CountAsync(a => a.EventDate >= cutoffDate30);

            var uniqueLocations = await _context.Set<LocationRcb>()
                .Select(l => l.Name)
                .Distinct()
                .CountAsync();

            var uniqueTags = await _context.Set<TagRcb>()
                .Select(t => t.Name)
                .Distinct()
                .CountAsync();

            var mostCommonLocations = await _context.Set<LocationRcb>()
                .GroupBy(l => l.Name.ToLower())
                .Select(g => new {
                    Location = g.First().Name,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            var mostCommonTags = await _context.Set<TagRcb>()
                .GroupBy(t => t.Name.ToLower())
                .Select(g => new {
                    Tag = g.First().Name,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            var stats = new
            {
                TotalAlerts = totalAlerts,
                AlertsLast7Days = alertsLast7Days,
                AlertsLast30Days = alertsLast30Days,
                UniqueLocations = uniqueLocations,
                UniqueTags = uniqueTags,
                MostCommonLocations = mostCommonLocations,
                MostCommonTags = mostCommonTags
            };

            return Ok(stats);
        }
    }
}