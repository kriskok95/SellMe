namespace SellMe.Data.Models
{
    using System.Collections.Generic;
    using Common;

    public class SubCategory : BaseDeletableModel<int>
    {
        public SubCategory()
        {
            Ads = new HashSet<Ad>();
        }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Ad> Ads { get; set; }
    }
}
