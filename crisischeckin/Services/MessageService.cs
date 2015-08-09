using System;
using System.Collections.Generic;
using System.Linq;
using Models;
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

        public void SendMessage(Message message, Person recipient, string senderDisplayName = null)
        {
            var messageRecipients = new List<MessageRecipient>
            {
                new MessageRecipient
                {
                    EmailAddress = recipient.Email,
                    Name = string.Format("{0} {1}", recipient.FirstName, recipient.LastName)
                }
            };
            _msgCoordinatorSvc.SendMessage(message, messageRecipients, senderDisplayName);
        }

        public void SendMessageToDisasterVolunteers(Message message, RecipientCriterion recipientCriterion, string senderDisplayName = null)
        {
            var volunteers = _adminSvc.GetVolunteersForDate(recipientCriterion.DisasterId, DateTime.Today, recipientCriterion.ClusterCoordinatorsOnly, recipientCriterion.CheckedInOnly).ToList();

            if (!volunteers.Any())
                return;

            var messageRecipients = new List<MessageRecipient>();
            foreach (var volunteer in volunteers)
            {
                messageRecipients.Add(new MessageRecipient
                                      {
                                          EmailAddress = volunteer.Email,
                                          PhoneNumber = volunteer.PhoneNumber,
                                          Name = string.Format("{0} {1}", volunteer.FirstName, volunteer.LastName)
                                      });
            }

            _msgCoordinatorSvc.SendMessage(message, messageRecipients, senderDisplayName);
        }
    }
}
