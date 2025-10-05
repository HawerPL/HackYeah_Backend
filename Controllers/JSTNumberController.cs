using HackYeah_Backend.Data;
using HackYeah_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HackYeah_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JSTNumberController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public JSTNumberController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/jstnumber/{location}
        [HttpGet("{location}")]
        public async Task<ActionResult<IEnumerable<JSTNumber>>> Get(string location)
        {
            var numbers = await _dbContext.jSTNumbers
                .Where(l => l.Location == location)
                .ToListAsync();

            if (numbers == null || numbers.Count == 0)
                return NotFound($"Brak danych dla miejscowości: {location}");

            return Ok(numbers);
        }
    }
}
