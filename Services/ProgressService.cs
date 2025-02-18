using Choresbuddy_dotnet.Data;
using Choresbuddy_dotnet.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Choresbuddy_dotnet.Services
{
    public class ProgressService : IProgressService
    {
        private readonly AppDbContext _context;

        public ProgressService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Progress>> GetAllProgressAsync()
        {
            return await _context.progresses.ToListAsync();
        }

        public async Task<Progress> GetProgressByChildIdAsync(int childId)
        {
            return await _context.progresses.FirstOrDefaultAsync(p => p.ChildId == childId);
        }

        public async Task<bool> UpdateProgressAsync(int childId, int tasksCompleted, int tasksMissed)
        {
            var progress = await _context.progresses.FirstOrDefaultAsync(p => p.ChildId == childId);
            if (progress == null)
            {
                progress = new Progress { ChildId = childId, TasksCompleted = tasksCompleted, TasksMissed = tasksMissed };
                _context.progresses.Add(progress);
            }
            else
            {
                progress.TasksCompleted += tasksCompleted;
                progress.TasksMissed += tasksMissed;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveProgressEntryAsync(int id)
        {
            var entry = await _context.progresses.FindAsync(id);
            if (entry == null) return false;

            _context.progresses.Remove(entry);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
