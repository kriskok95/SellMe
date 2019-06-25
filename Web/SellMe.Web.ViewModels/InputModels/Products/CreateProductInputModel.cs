using System.Collections.Generic;

namespace SellMe.Web.ViewModels.InputModels.Products
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    public class CreateProductInputModel
    {
        public CreateProductInputModel()
        {
            this.Images = new List<IFormFile>();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public string SubCategory { get; set; }

        public int Availability { get; set; }

        public string Condition { get; set; }

        [DataType(DataType.Upload)]
        public ICollection<IFormFile> Images { get; set; }
    }
}
