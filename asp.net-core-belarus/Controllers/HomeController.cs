using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using asp.net_core_belarus.Models;
using asp.net_core_belarus.Data;
using Microsoft.EntityFrameworkCore;

namespace asp.net_core_belarus.Controllers
{
    public class HomeController : Controller
    {
        private NorthwindDB dB;
        public HomeController()
        {
            dB = new NorthwindDB();
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Categories()
        {
            return View(dB.Categories);
        }

        public IActionResult Products()
        {
            return View(dB.Products.Include(c=>c.Category).Include(s=>s.Supplier));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
