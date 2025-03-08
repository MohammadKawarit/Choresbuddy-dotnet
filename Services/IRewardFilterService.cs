using Choresbuddy_dotnet.Models;
using Choresbuddy_dotnet.Models.Requests;

namespace Choresbuddy_dotnet.Services
{
    public interface IRewardFilterService
    {
        Task<IEnumerable<RewardFilter>> GetAllFiltersAsync();
        Task<RewardFilter?> GetFilterByParentIdAsync(int parentId);
        Task<RewardFilter> UpsertFilterAsync(RewardFilterRequest filter);
        Task<bool> RemoveFilterAsync(int parentId);
    }
}
