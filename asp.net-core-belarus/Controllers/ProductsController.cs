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
        /// <summary>
        /// Get list of products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return service.Products;
        }

        /// <summary>
        /// Create a product record
        /// </summary>
        /// <param name="product"></param>
        [HttpPost]
        public void Create([FromBody] Product product)
        {
            if (product!=null)
            {
                service.UpdateProduct(product);
            }
        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        [HttpPut("{id}")]
        public void Update(int id, [FromBody] Product product)
        {
            if (product != null)
            {
                service.UpdateProduct(product);
            }
        }
        /// <summary>
        /// Delete product record
        /// </summary>
        /// <param name="id"></param>
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
