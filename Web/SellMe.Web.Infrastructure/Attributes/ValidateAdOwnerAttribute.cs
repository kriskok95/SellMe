namespace SellMe.Web.Infrastructure.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ValidateAdOwnerAttribute : ValidationAttribute
    {
        private const string ValidationSenderFirstMessageErrorMessage = "You can't send the message because you are the owner of this ad!";
        private const string RecipientIdNotFoundErrorMessage = "Property with the given name doesn't exist!";

        private readonly string recipientId;

        public ValidateAdOwnerAttribute(string recipientId)
        {
            this.recipientId = recipientId;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var senderId = (string)value;
            var property = validationContext.ObjectType.GetProperty(recipientId);
            if (property == null)
                throw new ArgumentException(RecipientIdNotFoundErrorMessage);

            var recipientIdAsString = (string)property.GetValue(validationContext.ObjectInstance);

            if (senderId == recipientIdAsString)
            {
                return new ValidationResult(ValidationSenderFirstMessageErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
