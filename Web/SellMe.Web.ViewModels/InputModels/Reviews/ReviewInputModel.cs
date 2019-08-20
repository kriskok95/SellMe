namespace SellMe.Web.ViewModels.InputModels.Reviews
{
    using System.ComponentModel.DataAnnotations;

    public class ReviewInputModel
    {
        private const int ReviewMinimumLength = 5;
        private const string ReviewMinimumLengthErrorMessage = "The review should be at least {1} characters.";
        private const string RatingErrorMessage = "Please click the number of stars you want to rate this shop.";

        [Required]
        public string OwnerId { get; set; }

        [Required]
        public string CreatorId { get; set; }

        [Required]
        [MinLength(ReviewMinimumLength, ErrorMessage = ReviewMinimumLengthErrorMessage)]
        public string Content { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = RatingErrorMessage)]
        public int Rating { get; set; }
    }
}
