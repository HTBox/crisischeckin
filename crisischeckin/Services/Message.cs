namespace Services
{
    public class Message
    {
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public bool IsSMSMessage { get; set; }

        public Message(string subject, string body)
        {
            Subject = subject;
            Body = body;
        }
    }
}
