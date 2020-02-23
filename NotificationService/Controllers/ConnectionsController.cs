using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Hubs;

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionsController : ControllerBase
    {
        IConnectionManager _connectionManager;

        public ConnectionsController(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public IActionResult UserConnections()
        {
            return Ok(GetUserConnections());
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