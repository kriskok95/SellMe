namespace SellMe.Data.Models
{
    using Common;

    public class AdRejection : BaseDeletableModel<int>
    {
        public string Comment { get; set; }

        public int AdId { get; set; }
        public virtual Ad Ad { get; set; }
    }
}
