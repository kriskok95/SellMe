namespace SellMe.Web.ViewModels.InputModels.Messages
{
    using System.ComponentModel.DataAnnotations;

    using SellMe.Web.Infrastructure.Attributes;

    public class SendMessageInputModel
    {
        private const int MessageMinLength = 10;
        private const string MessageMinLengthErrorMessage = "The message should be with a minimum length of {1} characters.";

        [Required]
        [MinLength(MessageMinLength, ErrorMessage = MessageMinLengthErrorMessage)]
        public string Content { get; set; }
        
        [Required]
        [ValidateAdOwner("RecipientId")]
        public string SenderId { get; set; }

        [Required]
        public string RecipientId { get; set; }

        [Required]
        public int AdId { get; set; }
    }
}
