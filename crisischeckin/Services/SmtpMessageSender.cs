using System;
using System.Collections.Generic;
using System.Net.Mail;
using Services.Interfaces;

namespace Services
{
    public class SmtpMessageSender : IMessageSender
    {
        private readonly Func<SmtpClient> _smtpClientFactory;
        private readonly MailAddress _senderAddress;

        public SmtpMessageSender(Func<SmtpClient> smtpClientFactory, MailAddress senderAddress)
        {
            _smtpClientFactory = smtpClientFactory;
            _senderAddress = senderAddress;
        }

        public void SendMessage(Message message, IReadOnlyCollection<MessageRecipient> recipients)
        {
            using (var smtpClient = _smtpClientFactory())
            {
                foreach (var recipient in recipients)
                {
                    var recipientAddress = new MailAddress(recipient.EmailAddress, recipient.Name);
                    var mailMessage = new MailMessage(_senderAddress, recipientAddress)
                    {
                        Subject = message.Subject,
                        IsBodyHtml = true,
                        Body = message.Body
                    };
                    smtpClient.Send(mailMessage);
                }
            }
        }
    }
}