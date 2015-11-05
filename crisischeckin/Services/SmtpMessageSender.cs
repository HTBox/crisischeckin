using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using Common;
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

        public void SendMessage(Message message, IReadOnlyCollection<MessageRecipient> recipients, string senderDisplayName = null)
        {
            using (var smtpClient = _smtpClientFactory())
            {
                var fromAddress = !String.IsNullOrWhiteSpace(senderDisplayName)
                    ? new MailAddress(_senderAddress.Address, senderDisplayName)
                    : _senderAddress;

                foreach (var recipient in recipients)
                {
                    var recipientAddress = new MailAddress(recipient.EmailAddress, recipient.Name);
                    var mailMessage = new MailMessage(fromAddress, recipientAddress)
                    {
                        Subject = SubjectEnrichmentService.Enrich(message.Subject),
                        IsBodyHtml = true,
                        Body = message.Body
                    };
                    smtpClient.Send(mailMessage);
                }
            }
        }

    }
}