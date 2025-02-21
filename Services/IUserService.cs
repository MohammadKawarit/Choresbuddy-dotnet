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
        Task<User> AddChild(string name, string email, string password, int parentId, DateTime dob);
        Task<string> LoginUserAsync(string email, string password);
        Task<bool> UpdateUserAsync(int id, string name, string email, string password, DateTime dob);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<User>> GetChildrenAsync(int parentId);
        Task<int> GetUserPointsAsync(int childId);
        Task<int> GetUserBalanceAsync(int userId);
        Task<ChildProfileDto> GetChildProfileAsync(int childId);
    }
}