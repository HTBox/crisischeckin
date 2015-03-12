using System;
using Models;

namespace Services.Interfaces
{
    public interface IMessageService
    {
        void SendMessage(Person toVolunteer, Message message);
        void SendMessageToDisasterVolunteers(RecipientCriterion recipientCriterion, Message message);
    }
}
