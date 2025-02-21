namespace Choresbuddy_dotnet.Models
{
    public class ChildProfileDto
    {
        public int ChildId { get; set; }
        public string ChildName { get; set; }
        public List<Task> AvailableTasks { get; set; }
        public List<Task> LateTasks { get; set; }
        public List<Task> CompletedTasks { get; set; }
        public int Points { get; set; }
    }
}
