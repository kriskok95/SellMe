namespace SellMe.Data.Models
{
    using SellMe.Data.Common;
    using System.Collections.Generic;

    public class Condition : BaseDeletableModel<int>
    {
        public Condition()
        {
            this.Ads = new HashSet<Ad>();
        }

        public string Name { get; set; }

        public ICollection<Ad> Ads { get; set; }
    }
}
