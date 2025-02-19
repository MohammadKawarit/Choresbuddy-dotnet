using Choresbuddy_dotnet.Data;
using Choresbuddy_dotnet.Models;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

        public async Task<User> RegisterUserAsync(string name, string email, string password, string role)
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

            user.PasswordHash = ComputeSha256Hash(password);

            _context.users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<string> LoginUserAsync(string email, string password)
        {
            var user = await _context.users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null || user.PasswordHash != ComputeSha256Hash(password))
            {
                throw new Exception("Invalid email or password.");
            }
            return GenerateJwtToken(user);
        }

        public async Task<bool> UpdateUserAsync(int id, User user)
        {
            var existingUser = await _context.users.FindAsync(id);
            if (existingUser == null) return false;

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;

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

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
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

    }
}
