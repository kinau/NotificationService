using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationService.Hubs
{
    public interface IHubNotificationHelper
    {
        void SendNotificationToAll(string sender, string message);
        IEnumerable<string> GetOnlineUsers();
        Task SendNotificationParrallel(string sender, string username, string message);
    }
}
