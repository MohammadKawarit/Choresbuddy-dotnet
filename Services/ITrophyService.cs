using Choresbuddy_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Choresbuddy_dotnet.Services
{
    public interface ITrophyService
    {
        Task<IEnumerable<Trophy>> GetAllTrophiesAsync();
        Task<Trophy> GetTrophyByChildIdAsync(int childId);
        Task<Trophy> AssignTrophyAsync(Trophy trophy);
        Task<bool> RemoveTrophyAsync(int id);
    }
}
