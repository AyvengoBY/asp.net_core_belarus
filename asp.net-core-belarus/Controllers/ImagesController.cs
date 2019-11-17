using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using asp.net_core_belarus.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace asp.net_core_belarus.Controllers
{
    public class ImagesController : Controller
    {
        private INorthwindService service;
        private IConfiguration configuration;
        private ILogger logger;
        public ImagesController(INorthwindService service, IConfiguration config, ILogger<HomeController> log)
        {
            this.service = service;
            configuration = config;
            logger = log;
        }

        public IActionResult Index()
        {
            var files = Directory.GetFiles((Directory.GetCurrentDirectory() + "\\wwwroot\\images\\"),"*.jpg").Select(f=>Path.GetFileName(f));
            
            return View(files);
        }

        public IActionResult GetImage(string image_id)
        {
            string fileName = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\images\\", $"{ image_id}.jpg");
            if (System.IO.File.Exists(fileName))
            {
                FileStream resultStream = new FileStream(fileName, FileMode.Open);
                return new FileStreamResult(resultStream, "image/jpeg");
            }
            return NotFound();
        }
    }
}