namespace SellMe.Web.ViewModels.Products
{
    using System.Collections.Generic;

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
