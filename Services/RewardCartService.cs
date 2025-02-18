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

        public RewardCartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RewardCart>> GetAllRewardCartsAsync()
        {
            return await _context.rewardCarts.ToListAsync();
        }

        public async Task<RewardCart> GetRewardCartByIdAsync(int rewardCartId)
        {
            return await _context.rewardCarts.FirstOrDefaultAsync(rc => rc.RewardCartId == rewardCartId);
        }

        public async Task<RewardCart> AddRewardToCartAsync(RewardCart rewardCart)
        {
            _context.rewardCarts.Add(rewardCart);
            await _context.SaveChangesAsync();
            return rewardCart;
        }

        public async Task<bool> RemoveRewardFromCartAsync(int id)
        {
            var rewardCart = await _context.rewardCarts.FindAsync(id);
            if (rewardCart == null) return false;

            _context.rewardCarts.Remove(rewardCart);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveRewardAsync(int id)
        {
            var rewardCart = await _context.rewardCarts.FindAsync(id);
            if (rewardCart == null) return false;

            rewardCart.ParentApprovalStatus = "APPROVED";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeclineRewardAsync(int id)
        {
            var rewardCart = await _context.rewardCarts.FindAsync(id);
            if (rewardCart == null) return false;

            rewardCart.ParentApprovalStatus = "DECLINED";
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
