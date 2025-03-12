using Choresbuddy_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Choresbuddy_dotnet.Services
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetAllCartsAsync();
        Task<Cart> GetCartByChildIdAsync(int childId);
        Task<Cart> AddToCartAsync(Cart cart);
        Task<bool> RemoveFromCartAsync(int id);
        Task<List<RewardInCart>> GetCartRewards(int cartId);
    }
}
