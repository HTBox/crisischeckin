using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IMessageCoordinator
    {
        void SendMessage(Message message, List<MessageRecipient> recipients, string senderDisplayName = null);
    }
}