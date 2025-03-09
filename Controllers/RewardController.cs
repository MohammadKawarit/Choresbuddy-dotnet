using Choresbuddy_dotnet.Models;
using Choresbuddy_dotnet.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Choresbuddy_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RewardController : ControllerBase
    {
        private readonly IRewardService _rewardService;

        public RewardController(IRewardService rewardService)
        {
            _rewardService = rewardService;
        }

        // GET: api/reward
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reward>>> GetAllRewards()
        {
            return Ok(await _rewardService.GetAllRewardsAsync());
        }

        // GET: api/reward/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Reward>> GetReward(int id)
        {
            var reward = await _rewardService.GetRewardByIdAsync(id);
            if (reward == null)
                return NotFound();

            return Ok(reward);
        }

        // POST: api/reward (Create a new reward)
        [HttpPost]
        public async Task<ActionResult<Reward>> CreateReward(Reward reward)
        {
            var createdReward = await _rewardService.CreateRewardAsync(reward);
            return CreatedAtAction(nameof(GetReward), new { id = createdReward.RewardId }, createdReward);
        }

        // PUT: api/reward/{id} (Update a reward)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReward(int id, Reward reward)
        {
            if (id != reward.RewardId)
                return BadRequest("Reward ID mismatch.");

            var updated = await _rewardService.UpdateRewardAsync(id, reward);
            if (!updated) return NotFound();

            return NoContent();
        }

        // DELETE: api/reward/{id} (Delete a reward)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReward(int id)
        {
            var deleted = await _rewardService.DeleteRewardAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpGet("child/{childId}/available")]
        public async Task<IActionResult> GetAvailableRewardsForChild(int childId)
        {
            var rewards = await _rewardService.GetFilteredRewardsForChildAsync(childId);

            if (rewards == null || rewards.Count == 0)
            {
                return NotFound(new { message = "No rewards available based on parent filters." });
            }

            return Ok(rewards);
        }
    }
}
