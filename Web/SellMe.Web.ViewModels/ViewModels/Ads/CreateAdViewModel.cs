using SellMe.Web.ViewModels.ViewModels.Categories;

namespace SellMe.Web.ViewModels.ViewModels.Ads
{
    using System.Collections.Generic;

    public class CreateAdViewModel
    {
        public CreateAdViewModel()
        {
            this.Categories = new HashSet<CreateAdCategoryViewModel>();
        }

        public ICollection<CreateAdCategoryViewModel> Categories { get; set; }

        public ICollection<string> Conditions { get; set; }
    }
}
