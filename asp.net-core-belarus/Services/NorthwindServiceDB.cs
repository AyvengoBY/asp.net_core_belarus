using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp.net_core_belarus.Data;
using Microsoft.EntityFrameworkCore;

namespace asp.net_core_belarus.Services
{
    public class NorthwindServiceDB : INorthwindService
    {
        private readonly NorthwindDB db;

        public NorthwindServiceDB(NorthwindDB database)
        {
            this.db = database;
        }

        public IEnumerable<Category> Categories
        {
            get
            {
                return db.Categories;
            }
        }
        public IEnumerable<Product> Products
        {
            get
            {
                return db.Products;
            }
        }
        public IEnumerable<Supplier> Suppliers
        {
            get
            {
                return db.Suppliers;
            }
        }

        public void DeleteProduct(int id)
        {
            db.Products.Remove(db.Products.First(p => p.ProductID == id));
            db.SaveChanges();
        }

        public Product Product(int id)
        {
            return db.Products.Where(p => p.ProductID == id).FirstOrDefault();
        }

        public IEnumerable<Product> GetProducts(int maxProducts = 0)
        {
            List<Product> result;
            if (maxProducts > 0)
            {
                result = db.Products.Include(c => c.Category).Include(s => s.Supplier).Take(maxProducts).ToList();
            }
            else
            {
                result = db.Products.Include(c => c.Category).Include(s => s.Supplier).ToList();
            }
            return result;
        }

        public void UpdateProduct(Product product)
        {
            db.Products.Update(product);
            db.SaveChanges();
        }
    }
}
