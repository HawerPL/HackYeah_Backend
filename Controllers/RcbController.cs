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
                .OrderByDescending(a => a.EventDate)
                .ToListAsync();

            return Ok(alerts);
        }

        // GET: api/Rcb/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rcb>> GetById(int id)
        {
            var alert = await _context.Rcbs.FindAsync(id);

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
                .Where(a => a.Location != null && a.Location.Contains(locationName))
                .OrderByDescending(a => a.EventDate)
                .ToListAsync();

            if (!alerts.Any())
            {
                return NotFound($"Nie znaleziono alertów dla lokalizacji: {locationName}");
            }

            return Ok(alerts);
        }

        // GET: api/Rcb/tag/{tagName}
        [HttpGet("tag/{tagName}")]
        public async Task<ActionResult<IEnumerable<Rcb>>> GetByTag(string tagName)
        {
            var alerts = await _context.Rcbs
                .Where(a => a.Tags != null && a.Tags.Contains(tagName))
                .OrderByDescending(a => a.EventDate)
                .ToListAsync();

            if (!alerts.Any())
            {
                return NotFound($"Nie znaleziono alertów z tagiem: {tagName}");
            }

            return Ok(alerts);
        }

        // GET: api/Rcb/recent?days=7
        [HttpGet("recent")]
        public async Task<ActionResult<IEnumerable<Rcb>>> GetRecent([FromQuery] int days = 7)
        {
            var cutoffDate = DateTime.Now.AddDays(-days);

            var alerts = await _context.Rcbs
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
                .Where(a =>
                    (a.Title != null && a.Title.Contains(query)) ||
                    (a.Description != null && a.Description.Contains(query))
                )
                .OrderByDescending(a => a.EventDate)
                .ToListAsync();

            return Ok(alerts);
        }

        // GET: api/Rcb/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount()
        {
            var count = await _context.Rcbs.CountAsync();
            return Ok(count);
        }
    }
}
