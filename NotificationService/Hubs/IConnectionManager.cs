using System.Collections.Generic;

namespace NotificationService.Hubs
{
    public interface IConnectionManager
    {
        void AddConnection(string username, string connectinoId);
        void RemoveConnection(string connectionId);
        HashSet<string> GetConnections(string username);
        IEnumerable<string> OnlineUsers { get; }
    }
}
