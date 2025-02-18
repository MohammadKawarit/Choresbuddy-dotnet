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
    }
}
