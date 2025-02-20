using Choresbuddy_dotnet.Models;
using Choresbuddy_dotnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Choresbuddy_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetUserNotifications(int userId)
        {
            return Ok(await _notificationService.GetUserNotificationsAsync(userId));
        }

        // POST: api/notification
        [HttpPost]
        public async Task<ActionResult<Notification>> CreateNotification(int userId, string message)
        {
            var notification = await _notificationService.AddNotificationAsync(userId, message);
            return CreatedAtAction(nameof(GetUserNotifications), new { userId = notification.UserId }, notification);
        }

        // PUT: api/notification/{notificationId}/read
        [HttpPut("{notificationId}/read")]
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            await _notificationService.MarkNotificationAsReadAsync(notificationId);
            return NoContent();
        }
    }
}
