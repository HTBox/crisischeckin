using System;
using System.Collections.Generic;
using Services.Interfaces;

namespace Services
{
    public class MessageService : IMessageService
    {
        private readonly IAdmin _adminSvc;
        private readonly IMessageCoordinator _msgCoordinatorSvc;

        public MessageService(IAdmin adminSvc, IMessageCoordinator msgCoordinatorSvc)
        {
            _msgCoordinatorSvc = msgCoordinatorSvc;
            _adminSvc = adminSvc;
        }

        public void SendMessageToDisasterVolunteers(RecipientCriterion recipientCriterion, Message message)
        {
            var volunteers = _adminSvc.GetVolunteersForDate(recipientCriterion.DisasterId, DateTime.Today);

            var messageRecipients = new List<MessageRecipient>();
            foreach (var volunteer in volunteers)
            {
                messageRecipients.Add(new MessageRecipient
                                      {
                                          EmailAddress = volunteer.Email,
                                          Name = string.Format("{0} {1}", volunteer.FirstName, volunteer.LastName)
                                      });
            }

            _msgCoordinatorSvc.SendMessage(message, messageRecipients);
        }
    }
}
