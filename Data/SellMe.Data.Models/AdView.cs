﻿namespace SellMe.Data.Models
{
    using Common;

    public class AdView : BaseModel<int>
    {
        public int AdId { get; set; }

        public virtual Ad Ad { get; set; }
    }
}
