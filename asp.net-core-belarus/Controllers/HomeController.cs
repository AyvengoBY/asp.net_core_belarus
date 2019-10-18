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

namespace asp.net_core_belarus.Controllers
{
    public class HomeController : Controller
    {
        private NorthwindDB dB;
        private IConfiguration configuration;
        private ILogger logger;
        public HomeController(NorthwindDB northwinddb, IConfiguration config, ILogger<HomeController> log)
        {
            dB = northwinddb;
            configuration = config;
            logger = log;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Categories()
        {
            return View(dB.Categories);
        }

        public void ThrowException()
        {
            throw new Exception("TEST EXCEPTION!");
        }

        public IActionResult Products()
        {
            IEnumerable<Product> model;
            var maxProd = configuration.GetValue<int>("MaximumProducts");

            if (maxProd > 0)
                    logger.LogInformation(Environment.NewLine + "INFO :  READ CONFIGURATION : Maximum products on Products page is {0}" + Environment.NewLine, maxProd);
                else
                    logger.LogInformation(Environment.NewLine + "INFO :  READ CONFIGURATION : All products will view on Products page" + Environment.NewLine, null);
            
            if (maxProd > 0)
            {
                model = dB.Products.Include(c => c.Category).Include(s => s.Supplier).Take(maxProd);
            }
            else
            {
                model = dB.Products.Include(c => c.Category).Include(s => s.Supplier);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ProductEdit(int id)
        {
            var model = new ProductEditViewModel();
            var product = dB.Products.FirstOrDefault<Product>(p => p.ProductID == id);

            if (product == null)
            {
                product = new Product
                {
                    CategoryID = dB.Categories.First().CategoryID,
                    SupplierID = dB.Suppliers.First().SupplierID,
                };
            }
            model.Category = dB.Categories.FirstOrDefault<Category>(c => c.CategoryID == product.CategoryID).CategoryName;
            model.Discontinued = product.Discontinued;
            model.ProductID = product.ProductID;
            model.ProductName = product.ProductName;
            model.Supplier = dB.Suppliers.FirstOrDefault<Supplier>(s => s.SupplierID == product.SupplierID).CompanyName;
            model.QuantityPerUnit = product.QuantityPerUnit;
            model.UnitPrice = product.UnitPrice;
            model.UnitsInStock = product.UnitsInStock;
            model.UnitsOnOrder = product.UnitsOnOrder;
            model.ReorderLevel = product.ReorderLevel;

            model.suppliers = dB.Suppliers.Select(s => s.CompanyName);
            model.categories = dB.Categories.Select(c => c.CategoryName);

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
                    Supplier = dB.Suppliers.FirstOrDefault(s => s.CompanyName == model.Supplier),
                    Category = dB.Categories.FirstOrDefault(c => c.CategoryName == model.Category),
                    CategoryID = dB.Categories.FirstOrDefault(c => c.CategoryName == model.Category).CategoryID,
                    SupplierID = dB.Suppliers.FirstOrDefault(s => s.CompanyName == model.Supplier).SupplierID
                };
                dB.Products.Update(product);
                dB.SaveChanges();

                return RedirectToAction("Products");
            }
            else
            {
                model.suppliers = dB.Suppliers.Select(s => s.CompanyName);
                model.categories = dB.Categories.Select(c => c.CategoryName);
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult ProductDelete(int id)
        {
            dB.Products.Remove(dB.Products.First(p => p.ProductID == id));
            dB.SaveChanges();
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
