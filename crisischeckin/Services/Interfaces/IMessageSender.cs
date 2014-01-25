using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IMessageSender
    {
        void SendMessage(Message message, IReadOnlyCollection<MessageRecipient> recipients);
    }
}