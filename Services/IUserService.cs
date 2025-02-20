using Choresbuddy_dotnet.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Choresbuddy_dotnet.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> RegisterUserAsync(string name, string email, string password, string role, int parentId);
        Task<string> LoginUserAsync(string email, string password);
        Task<bool> UpdateUserAsync(int id, User user);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<User>> GetChildrenAsync(int parentId);
        Task<int> GetUserPointsAsync(int childId);
        Task<int> GetUserBalanceAsync(int userId);
    }
}