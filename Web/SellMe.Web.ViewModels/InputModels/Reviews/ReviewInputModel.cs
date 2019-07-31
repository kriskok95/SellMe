namespace SellMe.Web.ViewModels.InputModels.Reviews
{
    using System.ComponentModel.DataAnnotations;

    public class ReviewInputModel
    {
        public const int ReviewMinimumLength = 5;
        public const string ReviewMinimumLengthErrorMessage = "The review should be at least {1} characters.";

        [Required]
        public string OwnerId { get; set; }

        [Required]
        public string CreatorId { get; set; }

        [Required]
        [MinLength(ReviewMinimumLength, ErrorMessage = ReviewMinimumLengthErrorMessage)]
        public string Content { get; set; }

        //TODO: Create custom validation attribute for the rating field.
        [Required]
        public int Rating { get; set; }
    }
}
