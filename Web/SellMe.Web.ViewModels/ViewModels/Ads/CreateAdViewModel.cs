namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using System.Collections.Generic;
    using Categories;

    public class CreateAdViewModel : BaseViewModel
    {
        public CreateAdViewModel()
        {
            Categories = new HashSet<CreateAdCategoryViewModel>();
        }

        public ICollection<CreateAdCategoryViewModel> Categories { get; set; }

        public ICollection<string> Conditions { get; set; }
    }
}
