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
            GetConnectionId().Wait();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            RemoveConnection().Wait();
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

        public async Task SendGroupMessage(string group, string message)
        {
            
            await Clients.Group(group).SendAsync("ReceiveGroupMessage", group, message);
        }

        public async Task GetConnectionId()
        {
            var httpContext = this.Context.GetHttpContext();
            var username = httpContext.Request.Query["username"];

            if (string.IsNullOrEmpty(username))
            {
                username = Context.User.Identity.Name;
            }

            Debug.WriteLine($"GetConnectionId() username={username}");

            if (!string.IsNullOrEmpty(username))
            {
                _connectionManager.AddConnection(username, Context.ConnectionId);
            }


            var group = httpContext.Request.Query["group"];

            if (!string.IsNullOrEmpty(group))
            {
                await AddToGroup(group);
            }
        }

        public async Task RemoveConnection()
        {
            var httpContext = this.Context.GetHttpContext();
            var group = httpContext.Request.Query["group"];

            if (!string.IsNullOrEmpty(group))
            {
                await RemoveFromGroup(group);
            }

            var username = httpContext.Request.Query["username"];

            if (string.IsNullOrEmpty(username))
            {
                username = Context.User.Identity.Name;
            }
            
            if (!string.IsNullOrEmpty(username))
            {
                _connectionManager.RemoveConnection(Context.ConnectionId);
            }
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", groupName, $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", groupName, $"{Context.ConnectionId} has left the group {groupName}.");
        }
    }
}
