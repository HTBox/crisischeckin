using System;

namespace crisicheckinweb.Wrappers
{
    public class UserNotActivatedException : Exception
    {
        public UserNotActivatedException()
        {
        }

        public UserNotActivatedException(string message) : base(message)
        {
        }
    }
}