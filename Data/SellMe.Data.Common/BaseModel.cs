namespace SellMe.Data.Common
{
    using System;

    public class BaseModel<TKey> : IAuditInfo 
    {
        public TKey Id { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }
    }
}
