using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net_core_belarus.Models
{
    public class UploadCategoryImageViewModel
    {

        public UploadCategoryImageViewModel() { }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ImageDataUrl { get; set; }
        public string UploadFileName { get; set; }

    }
}
