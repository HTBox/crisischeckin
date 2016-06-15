using System;
using System.IO;
using Twilio;

namespace Services.Mocks
{
    public class TwilioRestClientMock : TwilioRestClient
    {
        public TwilioRestClientMock(string accountSid, string authToken) : base(accountSid, authToken)
        {
        }

        public TwilioRestClientMock(string accountSid, string authToken, string accountResourceSid) : base(accountSid, authToken, accountResourceSid)
        {
        }

        public string SaveLocation { get; set; }

        public override void SendSmsMessage(string @from, string to, string body, Action<SMSMessage> callback)
        {
            string fileName = string.Format(@"{0}\{1}-{2:hhmmss}.txt", SaveLocation, @from.Trim(), DateTime.Now);
            if (File.Exists(fileName))
                File.Delete(fileName);

            File.WriteAllText(fileName, string.Format("To: {0} {1}Message: {2}", to, Environment.NewLine, body));
        }
    }
}