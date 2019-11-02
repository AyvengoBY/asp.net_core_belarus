using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

        public Stream GetCategoryImageJpg(int id)
        {
            Stream dbStream = new MemoryStream();
            dbStream.Write(db.Categories.FirstOrDefault(c => c.CategoryID == id).Picture, 78, db.Categories.FirstOrDefault(c => c.CategoryID == id).Picture.Length-78);
            var image = Image.FromStream(dbStream);
            Stream resultStream = new MemoryStream();
            image.Save(resultStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            resultStream.Position = 0;
            return resultStream;
        }

        public void CategoryUploadJpegImage(int id, string filename)
        {
            Category category = Category(id);
            FileStream fileStream = new FileStream(filename, FileMode.Open);
            var image = Image.FromStream(fileStream);
            Stream dbStream = new MemoryStream();
            image.Save(dbStream, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] buf = new byte[dbStream.Length + 78];
            dbStream.Position = 0;
            dbStream.Read(buf, 78,(int)dbStream.Length);
            category.Picture = buf;
            db.Categories.Update(category);
            db.SaveChanges();
        }

        public Category Category(int id)
        {
            return db.Categories.Where(c => c.CategoryID == id).FirstOrDefault();
        }

        public string GetCategoryImageAsDataUrl(int id)
        {
            Stream stream = GetCategoryImageJpg(id);
            stream.Position = 0;
            byte[] buf = new byte[stream.Length];
            stream.Read(buf, 0, (int)stream.Length);
            string imageBase64Data = Convert.ToBase64String(buf);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            return imageDataURL;
        }

    }
}
