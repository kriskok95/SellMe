namespace SellMe.Data.Models
{
    using System;
    using SellMe.Data.Common;
    using System.Collections.Generic;

    public class Promotion : BaseModel<int>
    {
        public string Type { get; set; }

        public DateTime ActiveTo { get; set; }

        public virtual ICollection<Ad> Ads { get; set; }
    }
}
