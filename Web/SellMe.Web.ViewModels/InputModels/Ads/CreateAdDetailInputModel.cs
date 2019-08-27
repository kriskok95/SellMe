namespace SellMe.Web.ViewModels.InputModels.Ads
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    public class CreateAdDetailInputModel
    {
        private const string PriceErrorMessage = "{0} can't be a negative number!";
        private const string AvailabilityErrorMessage = "{0} can't be a negative number!";
        private const int TitleMinLength = 5;
        private const int TitleMaxLength = 120;
        private const string TitleLengthErrorMessage = "{0} should be between {2} and {1} characters length!";
        private const int DescriptionMinLength = 20;
        private const string PriceMinValue = "0.1";
        private const string PriceMaxValue = "79228162514264337593543950335";
        private const int AvailabilityMinValue = 1;
        private const int AvailabilityMaxValue = int.MaxValue;

        public CreateAdDetailInputModel()
        {
            Images = new List<IFormFile>();
        }

        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = TitleLengthErrorMessage)]
        [Required]
        public string Title { get; set; }

        [MinLength(DescriptionMinLength)]
        [Required]
        public string Description { get; set; }

        [Required]
        [Range(typeof(decimal), PriceMinValue, PriceMaxValue, ErrorMessage = PriceErrorMessage)]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int SubCategoryId { get; set; }

        [Range(AvailabilityMinValue, AvailabilityMaxValue, ErrorMessage = AvailabilityErrorMessage)]
        [Required]
        public int Availability { get; set; }

        [Required]
        public int ConditionId { get; set; }

        //TODO: Add validations for image types
        [DataType(DataType.Upload)]
        public ICollection<IFormFile> Images { get; set; }
    }
}
