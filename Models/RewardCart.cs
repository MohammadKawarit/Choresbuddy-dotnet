using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Choresbuddy_dotnet.Models
{
    [Table("reward_cart")]
    public class RewardCart
    {
        [Key]
        [Column("reward_cart_id")]
        public int RewardCartId { get; set; }

        [Column("cart_id")]
        public int CartId { get; set; }

        [ForeignKey("CartId")]
        public Cart Cart { get; set; }

        [Column("reward_id")]
        public int RewardId { get; set; }

        [ForeignKey("RewardId")]
        public Reward Reward { get; set; }

        [Column("parent_approval_status")]
        public string ParentApprovalStatus { get; set; } = "PENDING"; // "PENDING", "APPROVED", "DECLINED"

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // **New Fields**
        [Column("approved_by")]
        public int? ApprovedBy { get; set; } // Tracks which parent approved the reward

        public User? ApprovedParent { get; set; } // Navigation Property

        [Column("canceled")]
        public bool Canceled { get; set; } = false; // Allows child to cancel request
    }
}
