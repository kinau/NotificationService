using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Hubs
{
    public class HubNotificationHelper : IHubNotificationHelper
    {
        IHubContext<NotificationHub> Context { get; }

        private readonly IConnectionManager _connectionManager;

        public HubNotificationHelper(IHubContext<NotificationHub> context, IConnectionManager connectionManager)
        {
            Context = context;
            _connectionManager = connectionManager;
        }

        public async Task SendNotification(string sender, string username, string message)
        {
            HashSet<string> connections = _connectionManager.GetConnections(username);

            try
            {
                if (connections != null && connections.Count > 0)
                {

                    try
                    {
                        await Context.Clients.Clients(connections.ToList()).SendAsync("ReceiveMessage", sender, message);
                    }
                    catch
                    {
                        throw new Exception("ERROR: No connections found");
                    }

                }
            }
            catch
            {
                throw new Exception("ERROR");
            }
        }

        public void SendNotificationToAll(string sender, string message)
        {
            Context.Clients.All.SendAsync("ReceiveMessage", sender,  message);
        }

        public IEnumerable<string> GetOnlineUsers()
        {
            return _connectionManager.OnlineUsers;
        }

        public async Task SendNotificationParrallel(string sender, string username, string message)
        {
            await SendNotification(sender, username, message);
        }
    }
}
