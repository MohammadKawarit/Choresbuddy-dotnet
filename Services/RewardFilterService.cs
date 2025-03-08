using Choresbuddy_dotnet.Data;
using Choresbuddy_dotnet.Models;
using Choresbuddy_dotnet.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace Choresbuddy_dotnet.Services
{
    public class RewardFilterService : IRewardFilterService
    {
        private readonly AppDbContext _context;

        public RewardFilterService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RewardFilter>> GetAllFiltersAsync()
        {
            return await _context.rewardFilter.ToListAsync();
        }

        public async Task<RewardFilter?> GetFilterByParentIdAsync(int parentId)
        {
            return await _context.rewardFilter.FirstOrDefaultAsync(f => f.ParentId == parentId);
        }

        public async Task<RewardFilter> UpsertFilterAsync(RewardFilterRequest filterRequest)
        {
            var existingFilter = await _context.rewardFilter.FirstOrDefaultAsync(f => f.ParentId == filterRequest.ParentId);

            var filter = new RewardFilter()
            {
                BannedKeywords = filterRequest.BannedKeywords,
                ParentId = filterRequest.ParentId,
                MinPrice = filterRequest.MinPrice,
                MaxPrice = filterRequest.MaxPrice
            };

            if (existingFilter == null)
            {
                // Insert new filter
                _context.rewardFilter.Add(filter);
            }
            else
            {
                // Update existing filter
                existingFilter.BannedKeywords = filter.BannedKeywords;
                existingFilter.MinPrice = filter.MinPrice;
                existingFilter.MaxPrice = filter.MaxPrice;
            }

            await _context.SaveChangesAsync();
            return filter;
        }

        public async Task<bool> RemoveFilterAsync(int parentId)
        {
            var filter = await _context.rewardFilter.FirstOrDefaultAsync(f => f.ParentId == parentId);
            if (filter == null) return false;

            _context.rewardFilter.Remove(filter);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
