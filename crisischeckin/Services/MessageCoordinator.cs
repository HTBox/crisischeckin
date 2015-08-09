using System;
using System.Collections.Generic;
using Services.Interfaces;

namespace Services
{
    public class MessageCoordinator : IMessageCoordinator
    {
        private readonly List<IMessageSender> _messageSenders;

        public MessageCoordinator(List<IMessageSender> messageSenders)
        {
            _messageSenders = messageSenders;
        }

        public void SendMessage(Message message, List<MessageRecipient> recipients, string senderDisplayName = null)
        {
            foreach (var messageSender in _messageSenders)
            {
                if (message.IsSMSMessage && !(messageSender is SMSMessageSender))
                    continue;

                messageSender.SendMessage(message, recipients, senderDisplayName);
            }
        }
    }
}