using HackYeah_Backend.Data;
using HackYeah_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace HackYeah_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BunkerController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public BunkerController(ApplicationDbContext dbContext) 
            {
                _dbContext = dbContext;
            }



        // GET: api/bunker/coordinates/{lon}/{lat}/{z}
        [HttpGet("coordinates/{lon}/{lat}/{z}")]
        public async Task<ActionResult<IEnumerable<Bunker>>> Get(double lon, double lat, double z)
        {
            var bunkers = await _dbContext.Bunkers.ToListAsync();

            var selectedBunkers = bunkers.Where(b =>
                b.x >= lon - lon * z && b.x <= lon + lon * z &&
                b.y >= lat - lat * z && b.y <= lat + z
            ).ToList();



            return Ok(selectedBunkers);
        }

    }
}
