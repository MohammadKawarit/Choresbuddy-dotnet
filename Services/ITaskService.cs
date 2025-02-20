using Choresbuddy_dotnet.Models;
using Choresbuddy_dotnet.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Choresbuddy_dotnet.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<Models.Task>> GetAllTasksAsync();
        Task<Models.Task> GetTaskByIdAsync(int taskId);
        Task<Models.Task> CreateTaskAsync(TaskRequest task);
        Task<bool> UpdateTaskAsync(int id, Models.Task task);
        Task<bool> DeleteTaskAsync(int id);
        Task<IEnumerable<Models.Task>> GetTasksForChildAsync(int childId);
        Task<bool> CompleteTaskAsync(int taskId);
        Task<bool> VerifyTaskCompletionAsync(int taskId, string status);
        Task<bool> AssignTaskToChildAsync(int taskId, int childId);
        Task<IEnumerable<Models.Task>> GetTasksForParentChildrenAsync(int parentId);
        Task<bool> SubmitTaskAsync(int taskId);
    }
}
