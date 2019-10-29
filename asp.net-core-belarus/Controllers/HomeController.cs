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
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using asp.net_core_belarus.Services;

namespace asp.net_core_belarus.Controllers
{
    public class HomeController : Controller
    {
        private INorthwindService service;
        private IConfiguration configuration;
        private ILogger logger;
        public HomeController(INorthwindService service, IConfiguration config, ILogger<HomeController> log)
        {
            this.service = service;
            configuration = config;
            logger = log;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Categories()
        {
            return View(service.Categories);
        }

        public void ThrowException()
        {
            throw new Exception("TEST EXCEPTION!");
        }

        public IActionResult Products()
        {
            IEnumerable<Product> model;
            var maximumProducts = Convert.ToInt32(configuration["MaximumProducts"]);

            logger.LogInformation(Environment.NewLine + "INFO :  READ CONFIGURATION : MaximumProducts : Value {0}" + Environment.NewLine, maximumProducts);

            model = service.GetProducts(maximumProducts);

            return View(model);
        }

        [HttpGet]
        public IActionResult ProductEdit(int id)
        {
            var model = new ProductEditViewModel();
            var product = service.Product(id);

            if (product == null)
            {
                product = new Product
                {
                    CategoryID = service.Products.First().CategoryID,
                    SupplierID = service.Products.First().SupplierID,
                };
            }
            model.Category = service.Categories.FirstOrDefault<Category>(c => c.CategoryID == product.CategoryID).CategoryName;
            model.Discontinued = product.Discontinued;
            model.ProductID = product.ProductID;
            model.ProductName = product.ProductName;
            model.Supplier = service.Suppliers.FirstOrDefault<Supplier>(s => s.SupplierID == product.SupplierID).CompanyName;
            model.QuantityPerUnit = product.QuantityPerUnit;
            model.UnitPrice = product.UnitPrice;
            model.UnitsInStock = product.UnitsInStock;
            model.UnitsOnOrder = product.UnitsOnOrder;
            model.ReorderLevel = product.ReorderLevel;

            model.suppliers = service.Suppliers.Select(s => s.CompanyName);
            model.categories = service.Categories.Select(c => c.CategoryName);

            return View(model);
        }
        [HttpPost]
        public IActionResult ProductEdit(ProductEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product
                {
                    Discontinued = model.Discontinued,
                    ProductID = model.ProductID,
                    ProductName = model.ProductName,
                    QuantityPerUnit = model.QuantityPerUnit,
                    ReorderLevel = model.ReorderLevel,
                    UnitPrice = model.UnitPrice,
                    UnitsInStock = model.UnitsInStock,
                    UnitsOnOrder = model.UnitsOnOrder,
                    Supplier = service.Suppliers.FirstOrDefault(s => s.CompanyName == model.Supplier),
                    Category = service.Categories.FirstOrDefault(c => c.CategoryName == model.Category),
                    CategoryID = service.Categories.FirstOrDefault(c => c.CategoryName == model.Category).CategoryID,
                    SupplierID = service.Suppliers.FirstOrDefault(s => s.CompanyName == model.Supplier).SupplierID
                };
                service.UpdateProduct(product);
                
                return RedirectToAction("Products");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            service.DeleteProduct(id);
            return RedirectToAction("Products");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var error = this.HttpContext.Features.Get<IExceptionHandlerFeature>().Error;
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            logger.LogError(error, Environment.NewLine + "ERROR :  Exception thrown on:{0}   Message:{1}  RequestID:{2}" + Environment.NewLine , exceptionHandlerPathFeature?.Path, error.Message, requestId);

            return View(new ErrorViewModel { RequestId = requestId });
        }
    }
}
