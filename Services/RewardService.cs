using Choresbuddy_dotnet.Data;
using Choresbuddy_dotnet.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Choresbuddy_dotnet.Services
{
    public class RewardService : IRewardService
    {
        private readonly AppDbContext _context;

        public RewardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reward>> GetAllRewardsAsync()
        {
            return await _context.rewards.ToListAsync();
        }

        public async Task<Reward> GetRewardByIdAsync(int rewardId)
        {
            return await _context.rewards.FirstOrDefaultAsync(r => r.RewardId == rewardId);
        }

        public async Task<Reward> CreateRewardAsync(Reward reward)
        {
            _context.rewards.Add(reward);
            await _context.SaveChangesAsync();
            return reward;
        }

        public async Task<bool> UpdateRewardAsync(int id, Reward reward)
        {
            var existingReward = await _context.rewards.FindAsync(id);
            if (existingReward == null) return false;

            existingReward.Name = reward.Name;
            existingReward.PointsRequired = reward.PointsRequired;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRewardAsync(int id)
        {
            var reward = await _context.rewards.FindAsync(id);
            if (reward == null) return false;

            _context.rewards.Remove(reward);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Reward>> GetFilteredRewardsForChildAsync(int childId)
        {
            // Get the child's parentId
            var child = await _context.users
                .Where(u => u.UserId == childId && u.Role == "Child")
                .FirstOrDefaultAsync();

            if (child == null || child.ParentId == null)
            {
                return new List<Reward>(); // No rewards if child or parent not found
            }

            int parentId = child.ParentId.Value;

            // Get parent's reward filter settings
            var filter = await _context.rewardFilter
                .Where(f => f.ParentId == parentId)
                .FirstOrDefaultAsync();

            if (filter == null)
            {
                // If no filters exist, return all rewards
                return await _context.rewards.ToListAsync();
            }

            // Parse banned keywords
            var bannedKeywords = (filter.BannedKeywords ?? "").Split(',')
                .Select(k => k.Trim().ToLower())
                .Where(k => !string.IsNullOrEmpty(k))
                .ToList();

            int minPrice = filter.MinPrice ?? 0;
            int maxPrice = filter.MaxPrice ?? int.MaxValue;

            // Query rewards based on parent filters
            var rewards = await _context.rewards
                .Where(r => r.PointsRequired >= minPrice && r.PointsRequired <= maxPrice)
                .ToListAsync();

            // Filter out rewards containing banned keywords
            var filteredRewards = rewards
                .Where(r => !bannedKeywords.Any(keyword => r.Name.ToLower().Contains(keyword)))
                .ToList();

            return filteredRewards;
        }
    }
}
