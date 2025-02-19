using Choresbuddy_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Choresbuddy_dotnet.Services
{
    public interface IRewardCartService
    {
        Task<IEnumerable<RewardCart>> GetAllRewardCartsAsync();
        Task<RewardCart> GetRewardCartByIdAsync(int rewardCartId);
        Task<RewardCart> AddRewardToCartAsync(RewardCart rewardCart);
        Task<bool> RemoveRewardFromCartAsync(int id);
        Task<bool> ApproveRewardAsync(int id);
        Task<bool> DeclineRewardAsync(int id);
        Task<bool> RequestRewardAsync(RewardCart rewardCart);
        Task<bool> ApproveRewardAsync(int rewardCartId, string status);
    }
}
