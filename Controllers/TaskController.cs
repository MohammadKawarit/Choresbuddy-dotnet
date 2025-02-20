using Choresbuddy_dotnet.Services;
using Microsoft.AspNetCore.Mvc;
using Choresbuddy_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Choresbuddy_dotnet.Models.Requests;

namespace Choresbuddy_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET: api/task
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetAllTasks()
        {
            return Ok(await _taskService.GetAllTasksAsync());
        }

        // GET: api/task/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Task>> GetTask(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        // POST: api/task (Create a new task)
        [HttpPost]
        public async Task<ActionResult<Models.Task>> CreateTask(TaskRequest task)
        {
            var createdTask = await _taskService.CreateTaskAsync(task);
            return CreatedAtAction(nameof(GetTask), new { id = createdTask.TaskId }, createdTask);
        }

        // PUT: api/task/{id} (Update a task)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, Models.Task task)
        {
            if (id != task.TaskId)
                return BadRequest("Task ID mismatch.");

            var updated = await _taskService.UpdateTaskAsync(id, task);
            if (!updated) return NotFound();

            return NoContent();
        }

        // DELETE: api/task/{id} (Delete a task)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var deleted = await _taskService.DeleteTaskAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpGet("child/{childId}")]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetTasksForChild(int childId)
        {
            var tasks = await _taskService.GetTasksForChildAsync(childId);
            return Ok(tasks);
        }

        [HttpPost("{taskId}/complete")]
        public async Task<IActionResult> CompleteTask(int taskId)
        {
            var success = await _taskService.CompleteTaskAsync(taskId);
            if (!success) return NotFound("Task not found");

            return Ok("Task marked as completed, pending review");
        }

        [HttpPut("{taskId}/verify")]
        public async Task<IActionResult> VerifyTask(int taskId, [FromBody] string status)
        {
            var success = await _taskService.VerifyTaskCompletionAsync(taskId, status);
            if (!success) return NotFound("Task not found");

            return Ok("Task verification updated");
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignTask([FromBody] TaskAssignmentRequest request)
        {
            var success = await _taskService.AssignTaskToChildAsync(request.TaskId, request.ChildId);
            if (!success) return BadRequest("Failed to assign task");

            return Ok("Task assigned successfully");
        }

        [HttpGet("{parentId}/tasks")]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetTasksForParentChildren(int parentId)
        {
            var tasks = await _taskService.GetTasksForParentChildrenAsync(parentId);

            if (tasks == null || !tasks.Any())
            {
                return NotFound("No tasks found for children.");
            }

            return Ok(tasks);
        }

        [HttpPatch("{taskId}/submit")]
        public async Task<IActionResult> SubmitTask(int taskId)
        {
            var result = await _taskService.SubmitTaskAsync(taskId);
            if (!result)
                return NotFound("Task not found or already submitted.");

            return Ok("Task submitted successfully.");
        }

        public class TaskAssignmentRequest
        {
            public int TaskId { get; set; }
            public int ChildId { get; set; }
        }
    }
}
