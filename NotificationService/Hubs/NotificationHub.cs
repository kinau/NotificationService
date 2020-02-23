using Microsoft.AspNetCore.SignalR;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Hubs
{
    public class NotificationHub : Hub
    {
        private IConnectionManager _connectionManager;

        public NotificationHub(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public override Task OnConnectedAsync()
        {
            GetConnectionId();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            RemoveConnection(Context.UserIdentifier);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task BroadcastMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);

        }

        public async Task SendMessage(string user, string message)
        {
            var connections = _connectionManager.GetConnections(user);
            await Clients.Clients(connections.ToList()).SendAsync("ReceiveMessage", Context.User.Identity.Name, message);

           
        }

        public string GetConnectionId()
        {
            var httpContext = this.Context.GetHttpContext();
            var username = httpContext.Request.Query["username"];

            if (string.IsNullOrEmpty(username))
            {
                username = Context.User.Identity.Name;
            }

            Debug.WriteLine($"GetConnectionId() username={username}");

            _connectionManager.AddConnection(username, Context.ConnectionId);

            return Context.ConnectionId;
        }

        public void RemoveConnection(string userId)
        {
            Debug.WriteLine($"RemoveConnection() userId={userId}");


            _connectionManager.RemoveConnection(Context.ConnectionId);
        }
    }
}
