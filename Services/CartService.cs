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

        public async Task<List<RewardInCart>> GetCartRewards(int cartId)
        {
            var rewardCarts = await _context.rewardCarts
            .Where(r => r.CartId == cartId)
            .ToListAsync();

            List<RewardInCart> rewards = new List<RewardInCart>();

            foreach (var rewardCart in rewardCarts)
            {
                var reward = await _context.rewards.FindAsync(rewardCart.RewardId);
                rewards.Add(new RewardInCart()
                {
                    Name = reward.Name,
                    Description = reward.Description,
                    CreatedAt = reward.CreatedAt,
                    ImageUrl = reward.ImageUrl,
                    ParentApprovalStatus = rewardCart.ParentApprovalStatus,
                    PointsRequired = reward.PointsRequired,
                    RewardId = reward.RewardId
                });
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
