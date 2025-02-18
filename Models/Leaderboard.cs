using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Choresbuddy_dotnet.Models
{
    [Table("leaderboard")]
    public class Leaderboard
    {
        [Key]
        [Column("leaderboard_id")]
        public int LeaderboardId { get; set; }

        [Column("child_id")]
        public int ChildId { get; set; }

        [ForeignKey("ChildId")]
        public User Child { get; set; }

        [Column("points")]
        public int Points { get; set; } = 0;

        [Column("rank")]
        public int Rank { get; set; }

        [Column("last_updated")]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
