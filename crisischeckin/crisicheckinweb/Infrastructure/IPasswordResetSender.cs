using System;

namespace crisicheckinweb.Infrastructure
{
    public interface IPasswordResetSender
    {
        void SendEmail(int userId, string passwordResetLink);
    }
}