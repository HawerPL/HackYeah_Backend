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



        // GET: api/bunker/coordinates/{lon}/{lat}
        [HttpGet("coordinates/{lon}/{lat}")]
        public async Task<ActionResult<IEnumerable<Bunker>>> Get(float lon, float lat)
        {
            var bunkers = await _dbContext.Bunkers.ToListAsync();

            var selectedBunkers = bunkers.Where(b =>
                b.x >= lon - lon * 0.5 && b.x <= lon + lon * 0.5 &&
                b.y >= lat - lat * 0.5 && b.y <= lat + 20
            ).ToList();



            return Ok(selectedBunkers);
        }

    }
}
