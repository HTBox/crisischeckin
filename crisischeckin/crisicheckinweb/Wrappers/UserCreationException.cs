using System;

namespace crisicheckinweb.Wrappers
{
    public class UserCreationException : Exception
    {
        public UserCreationException()
        {
        }

        public UserCreationException(string message) : base(message)
        {
        }
    }
}