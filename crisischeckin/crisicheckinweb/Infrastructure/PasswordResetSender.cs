using System;
using System.Net.Mail;
using Services.Interfaces;

namespace crisicheckinweb.Infrastructure
{
    public class PasswordResetSender : IPasswordResetSender
    {
        private readonly Func<SmtpClient> _smtpClientFactory;
        private readonly IVolunteerService _volunteerService;

        public PasswordResetSender(Func<SmtpClient> smtpClientFactory, IVolunteerService volunteerService)
        {
            _smtpClientFactory = smtpClientFactory;
            _volunteerService = volunteerService;
        }


        public void SendEmail(int userId, string passwordResetLink)
        {
            var person = _volunteerService.FindByUserId(userId);
            if (person != null)
            {
                // TODO Refactor to use Services.MessageSender etc.
                using (var smtpClient = _smtpClientFactory())
                {
                    var message = new MailMessage();
                    message.From = new MailAddress("noreply@crisischeckin.com", "CrisisCheckin");
                    message.To.Add(new MailAddress(person.Email));
                    message.Subject = "CrisisCheckin - Password reset";
                    message.IsBodyHtml = true;
                    message.Body = String.Format(@"
                        <p>Click on the following link to reset your password: <a href='{0}'>{0}</a></p>", passwordResetLink);

                    smtpClient.Send(message);
                }
            }
        }
    }
}