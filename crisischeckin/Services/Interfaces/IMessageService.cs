using System;

namespace Services.Interfaces
{
    public interface IMessageService
    {
        void SendMessageToDisasterVolunteers(RecipientCriterion recipientCriterion, Message message);
    }
}
