namespace SellMe.Data.Models
{
    using System.Collections.Generic;
    using Common;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            SubCategories = new HashSet<SubCategory>();
            Ads = new HashSet<Ad>();
        }

        public string Name { get; set; }

        public string FontAwesomeIcon { get; set; }

        public virtual ICollection<SubCategory> SubCategories { get; set; }

        public virtual ICollection<Ad> Ads { get; set; }
    }
}
