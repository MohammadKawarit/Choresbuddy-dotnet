using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Choresbuddy_dotnet.Models
{
    [Table("progress")]
    public class Progress
    {
        [Key]
        [Column("progress_id")]
        public int ProgressId { get; set; }

        [Column("child_id")]
        public int ChildId { get; set; }

        [ForeignKey("ChildId")]
        public User Child { get; set; }

        [Column("tasks_completed")]
        public int TasksCompleted { get; set; } = 0;

        [Column("tasks_pending")]
        public int TasksPending { get; set; } = 0;

        [Column("tasks_missed")]
        public int TasksMissed { get; set; } = 0;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
