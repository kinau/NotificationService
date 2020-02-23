using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NotificationService.Hubs;

namespace NotificationService
{
    public class IndexModel : PageModel
    {
        private IConnectionManager _connectionManager;

        public IndexModel(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public List<SelectListItem> Options;

        public void OnGet()
        {
            Options = _connectionManager.OnlineUsers.Select(x => new SelectListItem()
            {
                Text = x,
                Value = x,
            }).ToList();
        }
    }
}