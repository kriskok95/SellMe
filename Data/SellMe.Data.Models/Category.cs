namespace SellMe.Data.Models
{
    using System.Collections.Generic;
    using SellMe.Data.Common;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.SubCategories = new HashSet<SubCategory>();
            this.Ads = new HashSet<Ad>();
        }

        public string Name { get; set; }

        public string FontAwesomeIcon { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }

        public ICollection<Ad> Ads { get; set; }
    }
}
