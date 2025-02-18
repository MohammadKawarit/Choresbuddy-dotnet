using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Choresbuddy_dotnet.Models
{
    [Table("trophies")]
    public class Trophy
    {
        [Key]
        [Column("trophy_id")]
        public int TrophyId { get; set; }

        [Column("child_id")]
        public int ChildId { get; set; }

        [ForeignKey("ChildId")]
        public User Child { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("earned_at")]
        public DateTime EarnedAt { get; set; } = DateTime.UtcNow;
    }
}
