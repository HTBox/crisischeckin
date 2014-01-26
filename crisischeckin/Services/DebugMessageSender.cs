using System;
using System.Collections.Generic;
using System.Diagnostics;
using Services.Interfaces;

namespace Services
{
    public class DebugMessageSender : IMessageSender
    {
        public void SendMessage(Message message, IReadOnlyCollection<MessageRecipient> recipients)
        {
            foreach (var messageRecipient in recipients)
            {
                Debug.WriteLine("To: {0} <{1}>\r\nSubject: {2}\r\nMessage: {3}",
                    messageRecipient.Name, messageRecipient.EmailAddress, message.Subject, message.Body);
            }
        }
    }
}