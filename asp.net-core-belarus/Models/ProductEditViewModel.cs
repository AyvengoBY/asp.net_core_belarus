using asp.net_core_belarus.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net_core_belarus.Models
{
    public class ProductEditViewModel
    {
        public Product product;

        public int ProductID { get; set; }

        [Required]
        [StringLength(40)]
        public string ProductName { get; set; }

        public string Supplier { get; set; }

        public string Category { get; set; }

        [StringLength(20)]
        public string QuantityPerUnit { get; set; }

        [Range(0,Double.MaxValue,ErrorMessage ="Must be positive")]
        public decimal? UnitPrice { get; set; }

        [Range(0, Double.MaxValue, ErrorMessage = "Must be positive")]
        public short? UnitsInStock { get; set; }

        [Range(0, Double.MaxValue, ErrorMessage = "Must be positive")]
        public short? UnitsOnOrder { get; set; }

        [Range(0, Int16.MaxValue, ErrorMessage = "Must be positive")]
        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }


        public IEnumerable<string> suppliers;
        public IEnumerable<string> categories;
    }
}
