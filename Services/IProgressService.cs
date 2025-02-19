using Choresbuddy_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Choresbuddy_dotnet.Services
{
    public interface IProgressService
    {
        Task<IEnumerable<Progress>> GetAllProgressAsync();
        Task<Progress> GetProgressByChildIdAsync(int childId);
        Task<bool> UpdateProgressAsync(int childId, int tasksCompleted, int tasksMissed);
        Task<bool> RemoveProgressEntryAsync(int id);
        Task<Progress> GetChildProgressAsync(int childId);
    }
}
