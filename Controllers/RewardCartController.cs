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

        // PUT: api/rewardcart/{childId}/decline (Decline reward)
        [HttpPut("{childId}/decline")]
        public async Task<IActionResult> DeclineReward(int childId)
        {
            var declined = await _rewardCartService.DeclineRewardAsync(childId);
            if (!declined) return NotFound();
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> RequestReward([FromBody] RewardCart rewardCart)
        {
            var success = await _rewardCartService.RequestRewardAsync(rewardCart);
            if (!success) return BadRequest("Failed to request reward");

            return Ok("Reward request submitted");
        }

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart(int childId, int rewardId)
        {
            var rewardCart = await _rewardCartService.AddRewardToCartAsync(childId, rewardId);
            if (rewardCart == null) return BadRequest("Insufficient points or invalid reward.");
            return Ok(rewardCart);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveFromCart(int cartId, int rewardId)
        {
            var success = await _rewardCartService.RemoveRewardFromCartAsync(cartId, rewardId);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPost("submit/{childId}")]
        public async Task<IActionResult> SubmitCart(int childId)
        {
            var success = await _rewardCartService.SubmitCartForApprovalAsync(childId);
            if (!success) return BadRequest("Failed to submit cart.");
            return Ok("Cart submitted for approval.");
        }

        [HttpPost("approve/{childId}")]
        public async Task<IActionResult> ApproveCart(int childId)
        {
            var success = await _rewardCartService.ApproveCartAsync(childId);
            if (!success) return BadRequest("Approval failed or insufficient balance.");
            return Ok("Cart approved and balance deducted.");
        }

    }
}
