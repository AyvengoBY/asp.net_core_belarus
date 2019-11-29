using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using asp.net_core_belarus.Data;
using asp.net_core_belarus.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace asp_net_core_belarus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private INorthwindService service;
        private IConfiguration configuration;
        private ILogger logger;

        public CategoriesController(INorthwindService service, IConfiguration config, ILogger<CategoriesController> log)
        {
            this.service = service;
            configuration = config;
            logger = log;
        }
        // GET: api/Categories
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            return service.Categories;
        }
        // GET: api/Categories
        [HttpGet("{id}")]
        public Stream GetImage(int id)
        {
            return service.GetCategoryImageJpg(id);
        }
        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public void UpdateImage(int id, [FromBody] Stream image)
        {
            if (image != null)
            {
                service.CategoryUploadImage(id, image);
            }
        }

    }
}
