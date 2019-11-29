using System;
using System.Collections.Generic;
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
    public class ProductsController : ControllerBase
    {
        private INorthwindService service;
        private IConfiguration configuration;
        private ILogger logger;

        public ProductsController(INorthwindService service, IConfiguration config, ILogger<ProductsController> log)
        {
            this.service = service;
            configuration = config;
            logger = log;
        }
        // GET: api/Products
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return service.Products;
        }

        // POST: api/Products
        [HttpPost]
        public void Create([FromBody] Product product)
        {
            if (product!=null)
            {
                service.UpdateProduct(product);
            }
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public void Update(int id, [FromBody] Product product)
        {
            if (product != null)
            {
                service.UpdateProduct(product);
            }
        }
        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var product = service.Product(id);
            if (product != null)
            {
                service.DeleteProduct(id);
            }
        }
    }
}
