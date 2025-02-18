using Choresbuddy_dotnet.Models;
using Choresbuddy_dotnet.Services;
using Microsoft.AspNetCore.Mvc;

namespace Choresbuddy_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrophyController : ControllerBase
    {
        private readonly ITrophyService _trophyService;

        public TrophyController(ITrophyService trophyService)
        {
            _trophyService = trophyService;
        }

        // GET: api/trophy
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trophy>>> GetAllTrophies()
        {
            return Ok(await _trophyService.GetAllTrophiesAsync());
        }

        // GET: api/trophy/{childId}
        [HttpGet("{childId}")]
        public async Task<ActionResult<Trophy>> GetTrophy(int childId)
        {
            var trophy = await _trophyService.GetTrophyByChildIdAsync(childId);
            if (trophy == null)
                return NotFound();

            return Ok(trophy);
        }

        // POST: api/trophy (Assign a trophy)
        [HttpPost]
        public async Task<ActionResult<Trophy>> AssignTrophy(Trophy trophy)
        {
            var createdTrophy = await _trophyService.AssignTrophyAsync(trophy);
            return CreatedAtAction(nameof(GetTrophy), new { childId = createdTrophy.ChildId }, createdTrophy);
        }

        // DELETE: api/trophy/{id} (Remove a trophy)
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveTrophy(int id)
        {
            var deleted = await _trophyService.RemoveTrophyAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
