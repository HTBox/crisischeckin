using System;
using Services.Interfaces;

namespace Services
{
    public class MessageService : IMessageService
    {
        private readonly IAdmin _adminSvc;

        public MessageService(IAdmin adminSvc)
        {
            _adminSvc = adminSvc;
        }

        public void SendMessageToDisasterVolunteers(RecipientCriterion recipientCriterion, Message message)
        {
            // Get volunteers for disaster where DateTime.Today between person.StartDate and person.EndDate
            // 
            var volunteers = _adminSvc.GetVolunteersForDate(recipientCriterion.DisasterId, DateTime.Today);
        }
    }
}
