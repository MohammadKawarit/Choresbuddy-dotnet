using Choresbuddy_dotnet.Models;
using Choresbuddy_dotnet.Services;
using Microsoft.AspNetCore.Mvc;

namespace Choresbuddy_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET: api/cart
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetAllCarts()
        {
            return Ok(await _cartService.GetAllCartsAsync());
        }

        // GET: api/cart/{childId}
        [HttpGet("{childId}")]
        public async Task<ActionResult<Cart>> GetCart(int childId)
        {
            var cart = await _cartService.GetCartByChildIdAsync(childId);
            if (cart == null)
                return NotFound();

            return Ok(cart);
        }

        // POST: api/cart (Add item to cart)
        [HttpPost]
        public async Task<ActionResult<Cart>> AddToCart(Cart cart)
        {
            var createdCart = await _cartService.AddToCartAsync(cart);
            return CreatedAtAction(nameof(GetCart), new { childId = createdCart.ChildId }, createdCart);
        }

        // DELETE: api/cart/{id} (Remove item from cart)
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var deleted = await _cartService.RemoveFromCartAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
