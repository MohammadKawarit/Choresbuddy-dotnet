using Choresbuddy_dotnet.Data;
using Choresbuddy_dotnet.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Choresbuddy_dotnet.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.Task>> GetAllTasksAsync()
        {
            return await _context.tasks.ToListAsync();
        }

        public async Task<Models.Task> GetTaskByIdAsync(int taskId)
        {
            return await _context.tasks.FirstOrDefaultAsync(t => t.TaskId == taskId);
        }

        public async Task<Models.Task> CreateTaskAsync(Models.Task task)
        {
            _context.tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> UpdateTaskAsync(int id, Models.Task task)
        {
            var existingTask = await _context.tasks.FindAsync(id);
            if (existingTask == null) return false;

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Points = task.Points;
            existingTask.Deadline = task.Deadline;
            existingTask.Status = task.Status;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.tasks.FindAsync(id);
            if (task == null) return false;

            _context.tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
