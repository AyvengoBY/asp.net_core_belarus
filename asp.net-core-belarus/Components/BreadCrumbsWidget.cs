using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace asp.net_core_belarus.Components
{
    public class BreadCrumbsWidget : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string s = HttpContext.Request.Path.ToString().Replace("/"," > ");
            s = Regex.Replace(s, @"> 0", " > Create New");
            s = Regex.Replace(s, @"> [0-9]", "");

            return View((object) s);
        }
    }
}
