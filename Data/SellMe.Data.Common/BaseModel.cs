using System;

namespace SellMe.Data.Common
{
    public class BaseModel<TKey> : IAuditInfo 
    {
        public TKey Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
