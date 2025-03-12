using Choresbuddy_dotnet.Data;
using Choresbuddy_dotnet.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Choresbuddy_dotnet.Services
{
    public class RewardCartService : IRewardCartService
    {
        private readonly AppDbContext _context;
        private readonly INotificationService _notificationService;

        public RewardCartService(AppDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<RewardCart> AddRewardToCartAsync(RewardCart rewardCart)
        {
            _context.rewardCarts.Add(rewardCart);
            await _context.SaveChangesAsync();
            return rewardCart;
        }

        public async Task<bool> ApproveRewardAsync(int id)
        {
            var rewardCart = await _context.rewardCarts.FindAsync(id);
            if (rewardCart == null) return false;

            rewardCart.ParentApprovalStatus = "APPROVED";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RequestRewardAsync(RewardCart rewardCart)
        {
            _context.rewardCarts.Add(rewardCart);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveRewardAsync(int rewardCartId, string status)
        {
            var rewardRequest = await _context.rewardCarts.FindAsync(rewardCartId);
            if (rewardRequest == null) return false;

            rewardRequest.ParentApprovalStatus = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RewardCart>> GetAllRewardCartsAsync()
        {
            return await _context.rewardCarts.Include(rc => rc.Reward).ToListAsync();
        }

        public async Task<RewardCart?> GetRewardCartByIdAsync(int rewardCartId)
        {
            return await _context.rewardCarts
                .Include(rc => rc.Reward)
                .FirstOrDefaultAsync(rc => rc.RewardCartId == rewardCartId);
        }

        public async Task<RewardCart?> AddRewardToCartAsync(int childId, int rewardId)
        {
            var child = await _context.users.FindAsync(childId);
            var reward = await _context.rewards.FindAsync(rewardId);

            if (child == null || reward == null || child.Points < reward.PointsRequired)
                return null;

            // Deduct points from the child
            child.Points -= reward.PointsRequired;

            // Find or create cart
            var cart = await _context.carts.FirstOrDefaultAsync(c => c.ChildId == childId);
            if (cart == null)
            {
                cart = new Cart { ChildId = childId };
                _context.carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            // Add reward to cart
            var rewardCart = new RewardCart
            {
                CartId = cart.CartId,
                RewardId = rewardId,
                ParentApprovalStatus = "PENDING"
            };

            _context.rewardCarts.Add(rewardCart);
            await _context.SaveChangesAsync();

            return rewardCart;
        }

        public async Task<bool> RemoveRewardFromCartAsync(int cartId, int rewardId)
        {
            var rewardCart = await _context.rewardCarts.Include(rc => rc.Reward).FirstOrDefaultAsync(rc => rc.CartId == cartId && rc.RewardId == rewardId);
            if (rewardCart == null) return false;

            // Restore child's points
            var cart = await _context.carts.FindAsync(rewardCart.CartId);
            if (cart != null)
            {
                var child = await _context.users.FindAsync(cart.ChildId);
                if (child != null)
                {
                    child.Points += rewardCart.Reward.PointsRequired;
                }
            }

            _context.rewardCarts.Remove(rewardCart);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SubmitCartForApprovalAsync(int childId)
        {
            var cart = await _context.carts.Include(c => c.Child).FirstOrDefaultAsync(c => c.ChildId == childId);
            if (cart == null) return false;

            var parentId = cart.Child.ParentId;
            if (parentId == null) return false;

            var rewardCarts = await _context.rewardCarts.Where(r => r.CartId == cart.CartId && r.ParentApprovalStatus == "PENDING")
            .ToListAsync();
            foreach ( var rewardCart in rewardCarts )
            {
                rewardCart.ParentApprovalStatus = "SUBMITTED";
            }

            await _notificationService.AddNotificationAsync(parentId.Value, $"{cart.Child.Name} submitted a reward cart for approval.");

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ApproveCartAsync(int childId)
        {
            var cart = await _context.carts.Include(c => c.Child).FirstOrDefaultAsync(c => c.ChildId == childId);
            if (cart == null) return false;

            var parent = await _context.users.FindAsync(cart.Child.ParentId);
            if (parent == null) return false;

            var rewardCarts = await _context.rewardCarts.Where(rc => rc.CartId == cart.CartId).ToListAsync();
            foreach (var item in rewardCarts)
            {
                item.Reward = await _context.rewards.FindAsync(item.RewardId);   
            }
            int totalCost = rewardCarts.Sum(rc => rc.Reward.PointsRequired);

            if (parent.Balance < totalCost) return false; // Not enough balance

            parent.Balance -= totalCost;

            foreach (var rewardCart in rewardCarts)
            {
                rewardCart.ParentApprovalStatus = "APPROVED";
            }
            await _notificationService.AddNotificationAsync(childId, $"Your parent has approved your reward!");

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeclineRewardAsync(int childId)
        {
            var cart = await _context.carts.Include(c => c.Child).FirstOrDefaultAsync(c => c.ChildId == childId);
            if (cart == null) return false;

            var parent = await _context.users.FindAsync(cart.Child.ParentId);
            if (parent == null) return false;

            var rewardCarts = await _context.rewardCarts.Where(rc => rc.CartId == cart.CartId).ToListAsync();
            foreach (var item in rewardCarts)
            {
                item.Reward = await _context.rewards.FindAsync(item.RewardId);
            }
            int totalCost = rewardCarts.Sum(rc => rc.Reward.PointsRequired);

            var child = await _context.users.FirstOrDefaultAsync(u => u.UserId == childId);

            child.Points += totalCost;

            foreach (var rewardCart in rewardCarts)
            {
                rewardCart.ParentApprovalStatus = "DECLINED";
            }
            await _notificationService.AddNotificationAsync(childId, $"Your parent has declined your reward!");

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
