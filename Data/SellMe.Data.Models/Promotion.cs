namespace SellMe.Data.Models
{
    using System;
    using SellMe.Data.Common;

    public class Promotion : BaseModel<int>
    {
        public string Type { get; set; }

        public int Updates { get; set; }

        public bool IsActive => ActiveTo > DateTime.UtcNow;

        public DateTime ActiveTo { get; set; }

        public int AdId { get; set; }
        public virtual Ad Ad { get; set; }
    }
}
