using System;
using System.ComponentModel.DataAnnotations;
using Resources;

namespace crisicheckinweb.Infrastructure.Attributes
{
    public class BasicPasswordAttribute : ValidationAttribute
    {
        public BasicPasswordAttribute()
        {
            ErrorMessage = DefaultErrorMessages.InvalidPasswordFormat;
        }

        public override bool IsValid(object value)
        {
            var stringValue = (string)value;

            // Some fields are not required
            if (string.IsNullOrWhiteSpace(stringValue)) return true;

            string errorMessage;
            if (!PasswordComplexity.IsValid(stringValue, out errorMessage))
            {
                ErrorMessage = errorMessage;
                return false;
            }

            return true;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!this.IsValid(value))
                return new ValidationResult(ErrorMessage);

            return null;
        }
    }
}