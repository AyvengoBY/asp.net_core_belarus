using System.ComponentModel.DataAnnotations.Schema;

namespace asp.net_core_belarus.Data
{
    public class Category
    {
        public virtual int CategoryID { get; set; }

        public virtual string CategoryName { get; set; }

        [Column(TypeName = "ntext")]
        public virtual string Description { get; set; }

        public virtual byte[] Picture { get; set; }
    }
}