using Choresbuddy_dotnet.Data;
using Choresbuddy_dotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace Choresbuddy_dotnet.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cart>> GetAllCartsAsync()
        {
            return await _context.carts.ToListAsync();
        }

        public async Task<Cart> GetCartByChildIdAsync(int childId)
        {
            return await _context.carts.FirstOrDefaultAsync(c => c.ChildId == childId);
        }

        public async Task<List<Reward>> GetCartRewards(int cartId)
        {
            var rewardCarts = await _context.rewardCarts
            .Where(r => r.CartId == cartId)
            .ToListAsync();

            List<Reward> rewards = new List<Reward>();

            foreach (var rewardCart in rewardCarts)
            {
                rewards.Add(await _context.rewards.FindAsync(rewardCart.RewardId));
            }

            return rewards;
        }

        public async Task<Cart> AddToCartAsync(Cart cart)
        {
            _context.carts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<bool> RemoveFromCartAsync(int id)
        {
            var cart = await _context.carts.FindAsync(id);
            if (cart == null) return false;

            _context.carts.Remove(cart);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
