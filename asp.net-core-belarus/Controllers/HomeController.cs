using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using asp.net_core_belarus.Models;
using asp.net_core_belarus.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace asp.net_core_belarus.Controllers
{
    public class HomeController : Controller
    {
        private NorthwindDB dB;
        private IConfiguration configuration;
        public HomeController(NorthwindDB northwinddb, IConfiguration config)
        {
            dB = northwinddb;
            configuration = config;
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
            IEnumerable<Product> model;
            var maxProd = configuration.GetValue<int>("MaximumProducts");
            if (maxProd>0)
            {
                model = dB.Products.Include(c => c.Category).Include(s => s.Supplier).Take(maxProd);
            }
            else
            {
                model = dB.Products.Include(c => c.Category).Include(s => s.Supplier);
            }
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
