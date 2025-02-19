using Choresbuddy_dotnet.Services;
using Microsoft.AspNetCore.Mvc;
using Choresbuddy_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Choresbuddy_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;

        public ProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }

        // GET: api/progress
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Progress>>> GetAllProgress()
        {
            return Ok(await _progressService.GetAllProgressAsync());
        }

        // GET: api/progress/{childId}
        [HttpGet("{childId}")]
        public async Task<ActionResult<Progress>> GetProgress(int childId)
        {
            var progress = await _progressService.GetProgressByChildIdAsync(childId);
            if (progress == null)
                return NotFound();

            return Ok(progress);
        }

        // POST: api/progress (Create or Update progress)
        [HttpPost]
        public async Task<IActionResult> UpdateProgress(int childId, int tasksCompleted, int tasksMissed)
        {
            var updated = await _progressService.UpdateProgressAsync(childId, tasksCompleted, tasksMissed);
            if (!updated) return BadRequest("Failed to update progress.");

            return NoContent();
        }

        // DELETE: api/progress/{id} (Remove progress entry)
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveProgressEntry(int id)
        {
            var deleted = await _progressService.RemoveProgressEntryAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpGet("child/{childId}")]
        [Authorize(Roles = "Parent,Child")]
        public async Task<ActionResult<Progress>> GetChildProgress(int childId)
        {
            var progress = await _progressService.GetChildProgressAsync(childId);
            if (progress == null) return NotFound("No progress data found for this child");

            return Ok(progress);
        }
    }
}
