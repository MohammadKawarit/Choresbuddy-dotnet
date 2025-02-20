using Choresbuddy_dotnet.Models;

namespace Choresbuddy_dotnet.Services
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId);
        Task<Notification> AddNotificationAsync(int userId, string message);
        System.Threading.Tasks.Task MarkNotificationAsReadAsync(int notificationId);
    }
}
