using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Choresbuddy_dotnet.Models
{
    [Table("rewards")]
    public class Reward
    {
        [Key]
        [Column("reward_id")]
        public int RewardId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("points_required")]
        public int PointsRequired { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
