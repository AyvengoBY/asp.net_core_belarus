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
            string spath = HttpContext.Request.Path.ToString();
            spath = Regex.Replace(spath, @"/[0-9]", "");
            SortedDictionary<string, string> path = new SortedDictionary<string,string>();
            
            while(spath.Contains("/"))
            {
                string name = Regex.Match(spath, @"\/([A-Z][a-z]+)+$").ToString().Replace("/","");
                path.Add(name, spath);
                spath = Regex.Replace(spath, @"\/([A-Z][a-z]+)+$", "");
            }
            return View(path);
        }
    }
}
