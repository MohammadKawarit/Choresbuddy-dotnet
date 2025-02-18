using Choresbuddy_dotnet.Data;
using Choresbuddy_dotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace Choresbuddy_dotnet.Services
{
    public class TrophyService : ITrophyService
    {
        private readonly AppDbContext _context;

        public TrophyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trophy>> GetAllTrophiesAsync()
        {
            return await _context.trophies.ToListAsync();
        }

        public async Task<Trophy> GetTrophyByChildIdAsync(int childId)
        {
            return await _context.trophies.FirstOrDefaultAsync(t => t.ChildId == childId);
        }

        public async Task<Trophy> AssignTrophyAsync(Trophy trophy)
        {
            _context.trophies.Add(trophy);
            await _context.SaveChangesAsync();
            return trophy;
        }

        public async Task<bool> RemoveTrophyAsync(int id)
        {
            var trophy = await _context.trophies.FindAsync(id);
            if (trophy == null) return false;

            _context.trophies.Remove(trophy);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
