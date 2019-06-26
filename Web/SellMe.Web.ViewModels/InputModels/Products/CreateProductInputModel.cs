namespace SellMe.Web.ViewModels.InputModels.Products
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    public class CreateProductInputModel
    {
        private const string PriceErrorMessage = "The price can't be a negative number!";
        private const string AvailabilityErrorMessage = "The availability can't be a negative number!";

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
        [Range(typeof(decimal), "0.1", "79228162514264337593543950335", ErrorMessage = PriceErrorMessage)]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string SubCategory { get; set; }

        [Range(typeof(int), "1", "2147483647", ErrorMessage = AvailabilityErrorMessage)]
        [Required]
        public int Availability { get; set; }

        [Required]
        public string Condition { get; set; }

        [DataType(DataType.Upload)]
        public ICollection<IFormFile> Images { get; set; }
    }
}
