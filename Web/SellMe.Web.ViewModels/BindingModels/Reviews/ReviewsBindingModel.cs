namespace SellMe.Web.ViewModels.BindingModels.Reviews
{
    using System.Collections.Generic;
    using InputModels.Reviews;
    using Services.Paging;
    using ViewModels;
    using ViewModels.Reviews;

    public class ReviewsBindingModel : BaseViewModel
    {
        public string OwnerId { get; set; }

        public string SenderId { get; set; }

        public string OwnerUsername{ get; set; }

        public List<int> Votes { get; set; }

        public double AverageVote { get; set; }

        public ReviewInputModel InputModel { get; set; }

        public PaginatedList<ReviewViewModel> ViewModels { get; set; }
    }
}
