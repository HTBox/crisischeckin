using System;
using System.Linq;
using System.Text.RegularExpressions;
using Resources;

namespace crisicheckinweb.Infrastructure
{
    public class PasswordComplexity
    {
        public static bool IsValid(string password, out string errorMessage)
        {
            return IsValid(password, null, out errorMessage);
        }

        public static bool IsValid(string password, string userName, out string errorMessage)
        {
            // Check for password length
            if (!Regex.IsMatch(password, @"^[^\s]{6,20}$"))
            {
                errorMessage = DefaultErrorMessages.InvalidPasswordFormat;
                return false;
            }

            // Check for equality to username
            if (userName != null && String.Compare(password, userName, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                errorMessage = DefaultErrorMessages.InvalidPasswordEqualToUserName;
                return false;
            }

            // Check for common words
            var commonWords = new[] { "password", "pass", "ht" };
            if (commonWords.Any(w => password.IndexOf(w, StringComparison.InvariantCultureIgnoreCase) >= 0))
            {
                errorMessage = DefaultErrorMessages.InvalidPasswordGenericUsed;
                return false;
            }

            // By the current rules the password is safe enough to be used.
            errorMessage = null;
            return true;
        }
    }
}