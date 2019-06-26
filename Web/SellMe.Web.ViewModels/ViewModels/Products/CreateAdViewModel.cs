using System.Collections.Generic;

namespace SellMe.Web.ViewModels.ViewModels.Products
{
    public class CreateAdViewModel
    {
        public CreateAdViewModel()
        {
            this.Categories = new HashSet<string>();
        }

        public ICollection<string> Categories { get; set; }

        public ICollection<string> Conditions { get; set; }
    }
}
