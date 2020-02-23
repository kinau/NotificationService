using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NotificationService
{
    public class GroupModel : PageModel
    {
        public List<SelectListItem> Options;

        public void OnGet()
        {
            Options = new List<SelectListItem>()
            {
                new SelectListItem("general", "general"),
                new SelectListItem("premium", "premium")
            };
        }
    }
}