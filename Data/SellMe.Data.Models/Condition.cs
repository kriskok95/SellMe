namespace SellMe.Data.Models
{
    using SellMe.Data.Common;
    using System.Collections.Generic;

    public class Condition : BaseDeletableModel<int>
    {
        public Condition()
        {
            this.Products = new HashSet<Product>();
        }

        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
