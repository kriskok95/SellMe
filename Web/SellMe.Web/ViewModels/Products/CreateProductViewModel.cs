using System.Collections.Generic;

namespace SellMe.Web.ViewModels.Products
{
    public class CreateProductViewModel
    {
        public CreateProductViewModel()
        {
            this.Categories = new HashSet<string>();
        }

        public ICollection<string> Categories { get; set; }

        public ICollection<string> Conditions { get; set; }
    }
}
