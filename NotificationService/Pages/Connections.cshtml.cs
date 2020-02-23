using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NotificationService.Controllers;
using NotificationService.Hubs;

namespace NotificationService
{
    public class ConnectionsModel : PageModel
    {

        readonly IConnectionManager _connectionManager;

        public string Message { get; private set; }

       public ConnectionsModel(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public void OnGet()
        {
            var resultList = GetUserConnections();
            Message = JsonConvert.SerializeObject(resultList);
        }

        public IEnumerable<UserConnectionModel> GetUserConnections()
        {
            var resultList = new List<UserConnectionModel>();

            foreach (var username in _connectionManager.OnlineUsers)
            {

                var connections = _connectionManager.GetConnections(username);

                foreach (var conn in connections)
                {
                    resultList.Add(new UserConnectionModel
                    {
                        UserName = username,
                        Connection = conn
                    });
                }
            }

            return resultList;
        }
    }
}