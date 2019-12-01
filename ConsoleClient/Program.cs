using asp.net_core_belarus.Data;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ConsoleClient
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            client.BaseAddress = new Uri("https://localhost:44365/Api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            IEnumerable<Product> products = null;
            IEnumerable<Category> categories = null;

            products = await GetProducts();

            if (products != null)
            {
                Console.WriteLine("List of products :");
                foreach (var product in products)
                {
                    Console.WriteLine($"{product.ProductID}     {product.ProductName}");
                }
            }
            else
            {
                Console.WriteLine("No products found or server error!");
            }
            Console.WriteLine("Press any key...");
            Console.ReadKey();

            categories = await GetCategories();

            if (categories != null)
            {
                Console.WriteLine("List of categories :");
                foreach (var category in categories)
                {
                    Console.WriteLine($"{category.CategoryID}     {category.CategoryName}");
                }
            }
            else
            {
                Console.WriteLine("No categories found or server error!");
            }
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        static async Task<IEnumerable<Product>> GetProducts()
        {
            IEnumerable<Product> products = null;
            HttpResponseMessage response = await client.GetAsync("Products");
            if (response.IsSuccessStatusCode)
            {
                products = await response.Content.ReadAsAsync<IEnumerable<Product>>();
            }
            return products;
        }
        static async Task<IEnumerable<Category>> GetCategories()
        {
            IEnumerable<Category> categories = null;
            HttpResponseMessage response = await client.GetAsync("Categories");
            if (response.IsSuccessStatusCode)
            {
                categories = await response.Content.ReadAsAsync<IEnumerable<Category>>();
            }
            return categories;
        }
    }
}
