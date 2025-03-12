using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Choresbuddy_dotnet.Models
{
    public class RewardInCart
    {
        public int RewardId { get; set; }

        public string? Name { get; set; }

        public int PointsRequired { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ParentApprovalStatus { get; set; } = "PENDING"; // "PENDING", SUBMITTED, "APPROVED", "DECLINED"
    }
}
