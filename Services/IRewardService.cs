using Choresbuddy_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Choresbuddy_dotnet.Services
{
    public interface IRewardService
    {
        Task<IEnumerable<Reward>> GetAllRewardsAsync();
        Task<Reward> GetRewardByIdAsync(int rewardId);
        Task<Reward> CreateRewardAsync(Reward reward);
        Task<bool> UpdateRewardAsync(int id, Reward reward);
        Task<bool> DeleteRewardAsync(int id);
    }
}
