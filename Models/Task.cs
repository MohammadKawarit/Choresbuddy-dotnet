using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Choresbuddy_dotnet.Models
{
    [Table("tasks")]
    public class Task
    {
        [Key]
        [Column("task_id")]
        public int TaskId { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("points")]
        public int Points { get; set; }

        [Column("deadline")]
        public DateTime? Deadline { get; set; }

        [Column("status")]
        public string Status { get; set; } = "TO_DO";

        [Column("assigned_to")]
        public int AssignedToId { get; set; }

        [ForeignKey("AssignedToId")]
        public int AssignedTo { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // **New Fields**
        [Column("parent_approval_status")]
        public string ParentApprovalStatus { get; set; } = "PENDING"; // "PENDING", "APPROVED", "REJECTED"

        [Column("verification_deadline")]
        public DateTime? VerificationDeadline { get; set; } // Parents must approve before this deadline
    }
}
