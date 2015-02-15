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

        public void SendMessage(Message message, List<MessageRecipient> recipients)
        {
            foreach (var messageSender in _messageSenders)
            {
                messageSender.SendMessage(message, recipients);
            }
        }
    }
}