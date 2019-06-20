namespace SellMe.Data.Models
{
    using SellMe.Data.Common;
    using System.Collections.Generic;

    public class SubCategory : BaseModel<int>
    {
        public SubCategory()
        {
            this.Products = new HashSet<Product>();
        }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
