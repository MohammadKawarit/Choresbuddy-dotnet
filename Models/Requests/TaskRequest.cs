namespace Choresbuddy_dotnet.Models.Requests
{
    public class TaskRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssignedTo { get; set; }
        public DateTime Deadline { get; set; }
        public int Points { get; set; }
    }
}
