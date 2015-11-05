using System;
using System.Collections.Generic;
using System.Diagnostics;
using Services.Interfaces;

namespace Services
{
    public class DebugMessageSender : IMessageSender
    {
        public void SendMessage(Message message, IReadOnlyCollection<MessageRecipient> recipients, string senderDisplayName = null)
        {
            foreach (var messageRecipient in recipients)
            {
                Debug.WriteLine("From:{0} To: {1} <{2}>\r\nSubject: {3}\r\nMessage: {4}",
                    senderDisplayName, messageRecipient.Name, messageRecipient.EmailAddress, message.Subject, message.Body);
            }
        }
    }
}