using System.ComponentModel.DataAnnotations;
using System.Linq;
using SellMe.Data;

namespace SellMe.Web.Infrastructure.Attributes
{
    public class ValidateAdOwnerFirstMessageAttribute : ValidationAttribute
    {
        private const string ValidationSenderFirstMessageErrorMessage = "You are the seller of this ad!";

        private readonly int adId;
        private readonly string recipientId;
        private readonly SellMeDbContext context;

        public ValidateAdOwnerFirstMessageAttribute(int adId, string recipientId, SellMeDbContext context)
        {
            this.adId = adId;
            this.recipientId = recipientId;
            this.context = context;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!this.context.Messages.Any(x =>
                x.AdId == adId && x.RecipientId == recipientId && x.SenderId == (string) value))
            {
                return new ValidationResult(ValidationSenderFirstMessageErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
