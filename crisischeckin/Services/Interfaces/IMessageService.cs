using System;
using Models;

namespace Services.Interfaces
{
    public interface IMessageService
    {
        void SendMessage(Message message, Person recipient, string senderDisplayName = null);
        void SendMessageToDisasterVolunteers(Message message, RecipientCriterion recipientCriterion, string senderDisplayName = null);
    }
}
