using Choresbuddy_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Choresbuddy_dotnet.Services
{
    public interface ILeaderboardService
    {
        Task<IEnumerable<Leaderboard>> GetAllLeaderboardsAsync();
        Task<Leaderboard> GetLeaderboardByChildIdAsync(int childId);
        Task<bool> UpdateLeaderboardAsync(int childId, int points);
        Task<bool> RemoveLeaderboardEntryAsync(int id);
    }
}
