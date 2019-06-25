namespace SellMe.Web.ViewModels.Products
{
    using Microsoft.AspNetCore.Http;

    public class CreateProductInputModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public string Subcategory { get; set; }

        public int Availability { get; set; }

        public string Condition { get; set; }

        public IFormFile AdImage { get; set; }
    }
}
