using System;
using System.Collections.Generic;
using asp.net_core_belarus.Controllers;
using asp.net_core_belarus.Data;
using asp.net_core_belarus.Models;
using asp.net_core_belarus.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index()
        {
            var controller = new HomeController(null, null, null);

            var result = controller.Index() as ViewResult;

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
        [TestMethod]
        public void Categories()
        {
            var mockService = new Mock<INorthwindService>();
            mockService.Setup(s => s.Categories).Returns(categories());

            var controller = new HomeController(mockService.Object, null, null);

            var result = controller.Categories() as ViewResult;
            var model = result.ViewData.Model as List<Category>;
            

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsInstanceOfType(result.Model, typeof(List<Category>));
            Assert.AreEqual(3, model.Count);
        }
        [TestMethod]
        public void ThrowException()
        {
            var controller = new HomeController(null, null, null);
            Assert.ThrowsException<Exception>(() => controller.ThrowException());
        }
        [TestMethod]
        public void Products()
        {
            var mockService = new Mock<INorthwindService>();
            mockService.Setup(s => s.GetProducts(2)).Returns(getProducts());
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(s => s["MaximumProducts"]).Returns("2");
            var mockLogger = new Mock<ILogger<HomeController>>();

            var controller = new HomeController(mockService.Object, mockConfiguration.Object, mockLogger.Object);

            var result = controller.Products() as ViewResult;
            var model = result.ViewData.Model as List<Product>;


            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsInstanceOfType(result.Model, typeof(List<Product>));
            Assert.AreEqual(2, model.Count);
        }
        [TestMethod]
        public void ProductEditGet()
        {
            var mockService = new Mock<INorthwindService>();
            mockService.Setup(s => s.Product(2)).Returns(new Product { ProductID=2, ProductName="Test product", SupplierID=1, CategoryID=1 });
            mockService.Setup(s => s.Categories).Returns(categories());
            mockService.Setup(s => s.Suppliers).Returns(suppliers());

            var mockConfiguration = new Mock<IConfiguration>();
            var mockLogger = new Mock<ILogger<HomeController>>();

            var controller = new HomeController(mockService.Object, mockConfiguration.Object, mockLogger.Object);

            var result = controller.ProductEdit(2) as ViewResult;
            var model = result.ViewData.Model as ProductEditViewModel;


            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsInstanceOfType(result.Model, typeof(ProductEditViewModel));
            Assert.AreEqual("Test category 1", model.Category);
        }
        [TestMethod]
        public void ProductEdit_ModelIsNotValid()
        {
            var mockService = new Mock<INorthwindService>();
            mockService.Setup(s => s.Categories).Returns(categories());
            mockService.Setup(s => s.Suppliers).Returns(suppliers());

            var mockConfiguration = new Mock<IConfiguration>();
            var mockLogger = new Mock<ILogger<HomeController>>();

            var controller = new HomeController(mockService.Object, mockConfiguration.Object, mockLogger.Object);
            controller.ModelState.AddModelError("ProductName", "Required");
            var result = controller.ProductEdit(new ProductEditViewModel()) as ViewResult;
            var model = result.ViewData.Model as ProductEditViewModel;

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsInstanceOfType(result.Model, typeof(ProductEditViewModel));
        }
        [TestMethod]
        public void ProductDelete()
        {
            var mockService = new Mock<INorthwindService>();
            mockService.Setup(s => s.DeleteProduct(55));
            
            var mockConfiguration = new Mock<IConfiguration>();
            var mockLogger = new Mock<ILogger<HomeController>>();

            var controller = new HomeController(mockService.Object, mockConfiguration.Object, mockLogger.Object);

            var result = controller.ProductDelete(55) as RedirectToActionResult;

            Assert.AreEqual("Products", result.ActionName);
        }


        private IEnumerable<Supplier> suppliers()
        {
            return new List<Supplier>()
            {
                new Supplier{  SupplierID=1, CompanyName="Test supplier 1"},
                new Supplier{  SupplierID=2, CompanyName="Test supplier 2"},
            };
        }

        private IEnumerable<Product> getProducts()
        {
            return new List<Product>()
            {
                new Product{ ProductID =1, ProductName = "Test product 1", CategoryID=55, SupplierID=66},
                new Product{ ProductID =2, ProductName = "Test product 2"},
            };
        }

        private IEnumerable<Category> categories()
        {
            return new List<Category>() 
            { 
                new Category{ CategoryID =1, CategoryName = "Test category 1"},
                new Category{ CategoryID =2, CategoryName = "Test category 2"},
                new Category{ CategoryID =3, CategoryName = "Test category 3"}
            };
        }


    }
}
