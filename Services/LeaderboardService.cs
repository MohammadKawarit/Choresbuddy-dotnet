using Choresbuddy_dotnet.Data;
using Choresbuddy_dotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace Choresbuddy_dotnet.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly AppDbContext _context;

        public LeaderboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Leaderboard>> GetAllLeaderboardsAsync()
        {
            return await _context.leaderboards.OrderByDescending(l => l.Points).ToListAsync();
        }

        public async Task<Leaderboard> GetLeaderboardByChildIdAsync(int childId)
        {
            return await _context.leaderboards.FirstOrDefaultAsync(lb => lb.ChildId == childId);
        }

        public async Task<bool> UpdateLeaderboardAsync(int childId, int points)
        {
            var leaderboardEntry = await _context.leaderboards.FirstOrDefaultAsync(l => l.ChildId == childId);
            if (leaderboardEntry == null)
            {
                leaderboardEntry = new Leaderboard { ChildId = childId, Points = points, LastUpdated = DateTime.UtcNow };
                _context.leaderboards.Add(leaderboardEntry);
            }
            else
            {
                leaderboardEntry.Points += points;
                leaderboardEntry.LastUpdated = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveLeaderboardEntryAsync(int id)
        {
            var entry = await _context.leaderboards.FindAsync(id);
            if (entry == null) return false;

            _context.leaderboards.Remove(entry);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
