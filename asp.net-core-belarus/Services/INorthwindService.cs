using asp.net_core_belarus.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net_core_belarus.Services
{
    public interface INorthwindService
    {
        IEnumerable<Category> Categories { get; }
        IEnumerable<Product> Products { get; }
        IEnumerable<Supplier> Suppliers { get; }
        IEnumerable<Product> GetProducts(int maxProducts = 0);
        Product Product(int id);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
    }
}
