using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Choresbuddy_dotnet.Models
{
    [Table("reward_filters")]
    public class RewardFilter
    {
        [Key]
        [Column("filter_id")]
        public int FilterId { get; set; }

        [Column("parent_id")]
        public int ParentId { get; set; }

        [ForeignKey("ParentId")]
        public User Parent { get; set; }

        [Column("banned_keywords")]
        public string? BannedKeywords { get; set; } // Store keywords as a comma-separated string

        [Column("min_price")]
        public int? MinPrice { get; set; } = 0;

        [Column("max_price")]
        public int? MaxPrice { get; set; } = 1000;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
