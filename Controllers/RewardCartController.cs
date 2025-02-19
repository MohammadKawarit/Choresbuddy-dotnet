using Choresbuddy_dotnet.Models;
using Choresbuddy_dotnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Choresbuddy_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RewardCartController : ControllerBase
    {
        private readonly IRewardCartService _rewardCartService;

        public RewardCartController(IRewardCartService rewardCartService)
        {
            _rewardCartService = rewardCartService;
        }

        // GET: api/rewardcart
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RewardCart>>> GetAllRewardCarts()
        {
            return Ok(await _rewardCartService.GetAllRewardCartsAsync());
        }

        // GET: api/rewardcart/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<RewardCart>> GetRewardCart(int id)
        {
            var rewardCart = await _rewardCartService.GetRewardCartByIdAsync(id);
            if (rewardCart == null)
                return NotFound();

            return Ok(rewardCart);
        }

        // POST: api/rewardcart (Add reward to cart)
        [HttpPost]
        public async Task<ActionResult<RewardCart>> AddRewardToCart(RewardCart rewardCart)
        {
            var createdRewardCart = await _rewardCartService.AddRewardToCartAsync(rewardCart);
            return CreatedAtAction(nameof(GetRewardCart), new { id = createdRewardCart.RewardCartId }, createdRewardCart);
        }

        // DELETE: api/rewardcart/{id} (Remove reward from cart)
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveRewardFromCart(int id)
        {
            var deleted = await _rewardCartService.RemoveRewardFromCartAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        // PUT: api/rewardcart/{id}/decline (Decline reward)
        [HttpPut("{id}/decline")]
        public async Task<IActionResult> DeclineReward(int id)
        {
            var declined = await _rewardCartService.DeclineRewardAsync(id);
            if (!declined) return NotFound();
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Child")]
        public async Task<IActionResult> RequestReward([FromBody] RewardCart rewardCart)
        {
            var success = await _rewardCartService.RequestRewardAsync(rewardCart);
            if (!success) return BadRequest("Failed to request reward");

            return Ok("Reward request submitted");
        }

        [HttpPut("{rewardCartId}/approve")]
        [Authorize(Roles = "Parent")]
        public async Task<IActionResult> ApproveReward(int rewardCartId, [FromBody] string status)
        {
            var success = await _rewardCartService.ApproveRewardAsync(rewardCartId, status);
            if (!success) return NotFound("Reward request not found");

            return Ok("Reward request updated");
        }
    }
}
