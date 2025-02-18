using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Choresbuddy_dotnet.Models
{
    [Table("users")] // ✅ Ensure table name matches exactly
    public class User
    {
        [Key]
        [Column("user_id")] // ✅ Ensure column name matches exactly
        public int UserId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string PasswordHash { get; set; }

        [Column("role")]
        public string Role { get; set; } // "Parent" or "Child"

        [Column("parent_id")]
        public int? ParentId { get; set; }

        public User? Parent { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
