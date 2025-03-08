using System.ComponentModel.DataAnnotations.Schema;

namespace Choresbuddy_dotnet.Models.Requests
{
    public class RewardFilterRequest
    {
        public int ParentId { get; set; }

        public string? BannedKeywords { get; set; } // Store keywords as a comma-separated string

        public int? MinPrice { get; set; }

        public int? MaxPrice { get; set; }
    }
}
