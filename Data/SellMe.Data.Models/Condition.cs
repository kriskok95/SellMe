namespace SellMe.Data.Models
{
    using System.Collections.Generic;
    using Common;

    public class Condition : BaseDeletableModel<int>
    {
        public Condition()
        {
            Ads = new HashSet<Ad>();
        }

        public string Name { get; set; }

        public virtual ICollection<Ad> Ads { get; set; }
    }
}
