using System;
using System.Collections.Generic;
using System.Net.Mail;
using Services.Interfaces;
using Models;
using System.Net;
using System.Configuration;

namespace Services
{
    public class SmtpMessageSender : IMessageSender
    {
        private readonly Func<SmtpClient> _smtpClientFactory;
        private readonly SmtpSettings _smtpSettings;

        public SmtpMessageSender(Func<SmtpClient> smtpClientFactory, SmtpSettings smtpSettings)
        {
            _smtpClientFactory = smtpClientFactory;
            _smtpSettings = smtpSettings;
        }

        public void SendMessage(Message message, IReadOnlyCollection<MessageRecipient> recipients)
        {

            using (var smtpClient = _smtpClientFactory())
            {
                var fromAddress = CreateAddress(string.Concat(message.Subject, " - Coordinator"), "no-reply@CrisisCheckin.com");

                foreach (var recipient in recipients)
                {
                    var recipientAddr = CreateAddress(recipient.Name, recipient.EmailAddress);
                    smtpClient.Send(fromAddress, recipientAddr, message.Subject, message.Body);
                }
            }
        }

        private static string CreateAddress(string name, string email)
        {
            return string.Format("\"{0}\" <{1}>", name, email);
        }

        public class SmtpSettings
        {
            public string SenderName { get; set; }
            public string SenderEmail { get; set; }
        }
    }
}