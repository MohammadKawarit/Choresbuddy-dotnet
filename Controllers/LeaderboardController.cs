using Choresbuddy_dotnet.Models;
using Choresbuddy_dotnet.Services;
using Microsoft.AspNetCore.Mvc;

namespace Choresbuddy_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardController(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        // GET: api/leaderboard
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Leaderboard>>> GetAllLeaderboards()
        {
            return Ok(await _leaderboardService.GetAllLeaderboardsAsync());
        }

        // GET: api/leaderboard/{childId}
        [HttpGet("{childId}")]
        public async Task<ActionResult<Leaderboard>> GetLeaderboard(int childId)
        {
            var leaderboard = await _leaderboardService.GetLeaderboardByChildIdAsync(childId);
            if (leaderboard == null)
                return NotFound();

            return Ok(leaderboard);
        }

        // POST: api/leaderboard (Add or Update Points)
        [HttpPost]
        public async Task<IActionResult> UpdateLeaderboard(int childId, int points)
        {
            var updated = await _leaderboardService.UpdateLeaderboardAsync(childId, points);
            if (!updated) return BadRequest("Failed to update leaderboard.");

            return NoContent();
        }

        // DELETE: api/leaderboard/{id} (Remove leaderboard entry)
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveLeaderboardEntry(int id)
        {
            var deleted = await _leaderboardService.RemoveLeaderboardEntryAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
