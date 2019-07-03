namespace SellMe.Data.Models
{
    using SellMe.Data.Common;
    using System.Collections.Generic;

    public class SubCategory : BaseDeletableModel<int>
    {
        public SubCategory()
        {
            this.Ads = new HashSet<Ad>();
        }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Ad> Ads { get; set; }
    }
}
