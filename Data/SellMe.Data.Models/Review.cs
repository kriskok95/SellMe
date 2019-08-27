namespace SellMe.Data.Models
{
    using Common;

    public class Review : BaseDeletableModel<int>
    {
        public int Rating { get; set; }

        public string Comment { get; set; }

        public string OwnerId{ get; set; }
        public virtual SellMeUser Owner{ get; set; }

        public string CreatorId { get; set; }
        public virtual SellMeUser Creator { get; set; }
    }
}
