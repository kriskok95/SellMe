namespace SellMe.Data.Models
{
    using SellMe.Data.Common;

    public class UpdateAd : BaseModel<int>
    {
        public int AdId { get; set; }

        public virtual Ad Ad { get; set; }
    }
}
