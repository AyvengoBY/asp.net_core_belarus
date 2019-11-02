using asp.net_core_belarus.Services;
using System.IO;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using asp.net_core_belarus.Models;
using asp.net_core_belarus.Data;

namespace asp.net_core_belarus.Controllers
{
    public class CategoriesController : Controller
    {
        private INorthwindService service;
        private IConfiguration configuration;
        private ILogger logger;
        public  CategoriesController(INorthwindService service, IConfiguration config, ILogger<HomeController> log)
        {
            this.service = service;
            configuration = config;
            logger = log;
        }

        public IActionResult Index()
        {
            return View(service.Categories);
        }
        
        public IActionResult GetImage(int id)
        {
            Stream resultStream = service.GetCategoryImageJpg(id);
            return  new FileStreamResult(resultStream, "image/jpeg");
            
        }
        [HttpGet]
        public IActionResult UploadImage(int id)
        {
            UploadCategoryImageViewModel model = new UploadCategoryImageViewModel();
            model.CategoryId = id;
            model.CategoryName = service.Category(id).CategoryName;
            model.ImageDataUrl = service.GetCategoryImageAsDataUrl(id);
            model.UploadFileName = string.Empty;
            return View(model);
        }

        [HttpPost]
        public IActionResult UploadImage(UploadCategoryImageViewModel model)
        {
            string fullPath = Path.Combine(Directory.GetCurrentDirectory()+"\\wwwroot\\images\\", model.UploadFileName);
            
            if (!System.IO.File.Exists(fullPath))
            {
                ModelState.AddModelError("UploadFileName", "File not found.");
                return View(model);
            }
            service.CategoryUploadJpegImage(model.CategoryId, fullPath);
            return new RedirectToActionResult("Index", "Categories",null);
        }
    }
}
