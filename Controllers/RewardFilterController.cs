using Choresbuddy_dotnet.Models;
using Choresbuddy_dotnet.Models.Requests;
using Choresbuddy_dotnet.Services;
using Microsoft.AspNetCore.Mvc;

namespace Choresbuddy_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RewardFilterController : ControllerBase
    {
        private readonly IRewardFilterService _rewardFilterService;

        public RewardFilterController(IRewardFilterService rewardFilterService)
        {
            _rewardFilterService = rewardFilterService;
        }

        // GET: api/rewardfilter
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RewardFilter>>> GetAllFilters()
        {
            return Ok(await _rewardFilterService.GetAllFiltersAsync());
        }

        // GET: api/rewardfilter/{parentId}
        [HttpGet("{parentId}")]
        public async Task<ActionResult<RewardFilter>> GetFilter(int parentId)
        {
            var filter = await _rewardFilterService.GetFilterByParentIdAsync(parentId);
            if (filter == null)
                return NotFound(new { message = "Filter not found for this parent." });

            return Ok(filter);
        }

        // POST: api/rewardfilter (Insert or Update if exists)
        [HttpPost]
        public async Task<ActionResult<RewardFilter>> UpsertFilter(RewardFilterRequest filter)
        {
            var upsertedFilter = await _rewardFilterService.UpsertFilterAsync(filter);
            return CreatedAtAction(nameof(GetFilter), new { parentId = upsertedFilter.ParentId }, upsertedFilter);
        }

        // DELETE: api/rewardfilter/{parentId} (Remove filter)
        [HttpDelete("{parentId}")]
        public async Task<IActionResult> RemoveFilter(int parentId)
        {
            var deleted = await _rewardFilterService.RemoveFilterAsync(parentId);
            if (!deleted) return NotFound(new { message = "Filter not found for deletion." });

            return NoContent();
        }
    }
}
