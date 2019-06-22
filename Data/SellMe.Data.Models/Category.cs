namespace SellMe.Data.Models
{
    using System.Collections.Generic;
    using SellMe.Data.Common;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.SubCategories = new HashSet<SubCategory>();
            this.Products = new HashSet<Product>();
        }

        public string Name { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
