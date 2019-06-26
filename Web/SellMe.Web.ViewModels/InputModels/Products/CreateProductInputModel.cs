namespace SellMe.Web.ViewModels.InputModels.Products
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;


    public class CreateProductInputModel
    {
        public CreateProductInputModel()
        {
            this.Images = new List<IFormFile>();
        }

        [MinLength(10)]
        [MaxLength(120)]
        [Required]
        public string Title { get; set; }

        [MinLength(20)]
        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string SubCategory { get; set; }

        [Required]
        public int Availability { get; set; }

        [Required]
        public string Condition { get; set; }

        [DataType(DataType.Upload)]
        public ICollection<IFormFile> Images { get; set; }
    }
}
