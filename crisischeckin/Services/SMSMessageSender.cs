using System;
using System.Collections.Generic;
using System.Diagnostics;
using Services.Interfaces;
using Twilio;

namespace Services
{
    public class SMSMessageSender : IMessageSender
    {
        private readonly string _senderPhone;
        private readonly Func<TwilioRestClient> _twilioClient;

        public SMSMessageSender(Func<TwilioRestClient> twilioClient, string senderPhone)
        {
            _twilioClient = twilioClient;
            _senderPhone = senderPhone;
        }

        public void SendMessage(Message message, IReadOnlyCollection<MessageRecipient> recipients, string senderDisplayName = null)
        {
            TwilioRestClient twilioClient = _twilioClient();

            foreach (MessageRecipient messageRecipient in recipients)
            {
                if (!string.IsNullOrEmpty(messageRecipient.PhoneNumber))
                {
                    twilioClient.SendSmsMessage(_senderPhone, messageRecipient.PhoneNumber, message.Body, t =>
                    {
                        if (t.RestException != null)
                        {
                            Debug.WriteLine(t.RestException.Code);
                            Debug.WriteLine(t.RestException.Status);
                            Debug.WriteLine(t.RestException.Message);
                        }
                    });
                }
            }
        }
    }
}