using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Resources;

namespace crisicheckinweb.Infrastructure.Attributes
{
    public class BasicPasswordAttribute : RegularExpressionAttribute
    {
        public BasicPasswordAttribute()
            : base(@"^[^\s]{6,20}$")
        {
            ErrorMessage = DefaultErrorMessages.InvalidPasswordFormat;
        }

        public override bool IsValid(object value)
        {
            if (!base.IsValid(value)) return false;

            var commonWords = new[] { "password", "pass", "ht" };
            var stringValue = (string)value;

            // Some fields are not required
            if (string.IsNullOrWhiteSpace(stringValue)) return true;

            if (commonWords.Any(w => stringValue.IndexOf(w, StringComparison.InvariantCultureIgnoreCase) >= 0))
            {
                ErrorMessage = DefaultErrorMessages.InvalidPasswordGenericUsed;
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