using Choresbuddy_dotnet.Data;
using Choresbuddy_dotnet.Models;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace Choresbuddy_dotnet.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.users.FindAsync(id);
        }

        public async Task<User> RegisterUserAsync(string name, string email, string password, string role, int parentId)
        {
            if (await _context.users.AnyAsync(u => u.Email == email))
            {
                throw new Exception("Email already exists.");
            }

            User user = new User()
            {
                Name = name,
                Email = email,
                Role = role
            };

            if (parentId != 0)
            {
                user.ParentId = parentId;
            }

            user.PasswordHash = ComputeSha256Hash(password);

            _context.users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> AddChild(string name, string email, string password, int parentId, DateTime dob)
        {
            if (await _context.users.AnyAsync(u => u.Email == email))
            {
                throw new Exception("Email already exists.");
            }

            User user = new User()
            {
                Name = name,
                Email = email,
                Role = "Child",
                ParentId = parentId,
                Dob = DateTime.SpecifyKind(dob, DateTimeKind.Utc)
            };

            user.PasswordHash = ComputeSha256Hash(password);

            _context.users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<string> LoginUserAsync(string email, string password)
        {
            var user = await _context.users.SingleOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            if (user == null || user.PasswordHash != ComputeSha256Hash(password))
            {
                throw new Exception("Invalid email or password.");
            }

            var response = new
            {
                userId = user.UserId,
                role = user.Role
            };

            return JsonSerializer.Serialize(response);
        }

        public async Task<bool> UpdateUserAsync(int id, string name, string email, string password, DateTime dob)
        {
            var existingUser = await _context.users.FindAsync(id);
            if (existingUser == null) return false;

            existingUser.Name = name;
            existingUser.Email = email;
            existingUser.Dob = DateTime.SpecifyKind(dob, DateTimeKind.Utc);
            
            if (!string.IsNullOrEmpty(password))
            {
                existingUser.PasswordHash = ComputeSha256Hash(password);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.users.FindAsync(id);
            if (user == null) return false;

            _context.users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public async Task<IEnumerable<User>> GetChildrenAsync(int parentId)
        {
            return await _context.users.Where(u => u.ParentId == parentId).ToListAsync();
        }
        public async Task<int> GetUserPointsAsync(int childId)
        {
            var user = await _context.users.FindAsync(childId);
            return user?.Points ?? 0;
        }

        public async Task<int> GetUserBalanceAsync(int userId)
        {
            var user = await _context.users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            return user.Balance;
        }

        public async Task<ChildProfileDto> GetChildProfileAsync(int childId)
        {
            // Get child details
            var child = await _context.users
                .Where(u => u.UserId == childId && u.Role == "Child")
                .FirstOrDefaultAsync();

            if (child == null)
            {
                return null; // Child not found
            }

            // Fetch tasks assigned to the child
            var tasks = await _context.tasks
                .Where(t => t.AssignedTo == childId)
                .ToListAsync();

            // Categorize tasks
            var availableTasks = tasks.Where(t => (t.Status == "TO_DO" || t.Status == "IN_PROGRESS") && t.Deadline > DateTime.UtcNow).ToList();
            var lateTasks = tasks.Where(t => (t.Status == "TO_DO" || t.Status == "IN_PROGRESS") && t.Deadline <= DateTime.UtcNow).ToList();
            var completedTasks = tasks.Where(t => t.Status == "COMPLETED").ToList();

            // Return DTO
            return new ChildProfileDto
            {
                ChildId = child.UserId,
                ChildName = child.Name,
                AvailableTasks = availableTasks,
                LateTasks = lateTasks,
                CompletedTasks = completedTasks,
                Points = child.Points
            };
        }

    }
}
