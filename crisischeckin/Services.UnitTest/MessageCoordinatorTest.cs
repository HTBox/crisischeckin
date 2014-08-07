using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Moq;
using Services.Interfaces;

namespace Services.UnitTest
{
    [TestFixture]
    public class MessageCoordinatorTest
    {
        [Test]
        public void CoordinatorShouldRunMessageRecipientsToSingleSender()
        {
            var fakeSender = new Mock<IMessageSender>();
            var testData = SetupTestObjects(fakeSender);

            InvokeSendMessageMethod(testData);

            VerifyMessageSentToRecipientsThroughSenders(testData.Message, testData.Recipients, fakeSender);
        }
 
        [Test]
        public void CoordinatorShouldRunMessageRecipientsToAllInjectedSenders()
        {
            var fakeSender = new Mock<IMessageSender>();
            var fakeSender2 = new Mock<IMessageSender>();
            var testData = SetupTestObjects(fakeSender, fakeSender2);

            InvokeSendMessageMethod(testData);

            VerifyMessageSentToRecipientsThroughSenders(testData.Message, testData.Recipients, fakeSender);
        }

        [Test]
        public void CoordinatorShouldHandleFailureGracefully()
        {
            var fakeSender = new Mock<IMessageSender>();
            var fakeSender2 = new Mock<IMessageSender>();
            var fakeSender3 = new Mock<IMessageSender>();
            var testData = SetupTestObjects(fakeSender, fakeSender2, fakeSender3);

            InvokeSendMessageMethod(testData);

            fakeSender2.Setup(s => s.SendMessage(testData.Message, testData.Recipients))
                .Throws<NullReferenceException>();

            VerifyMessageSentToRecipientsThroughSenders(testData.Message, testData.Recipients, fakeSender);
        }

        private void VerifyMessageSentToRecipientsThroughSenders(Message message,
            IReadOnlyCollection<MessageRecipient> recipients, params Mock<IMessageSender>[] fakeSenders)
        {
            foreach (var fakeSender in fakeSenders)
            {
                fakeSender.Verify(s => s.SendMessage(message, recipients),
                    Times.Once());
            }
        }

        private class TestObjects
        {
            public TestObjects(List<IMessageSender> messageSenders, MessageCoordinator coordinator, Message message,
                List<MessageRecipient> recipients)
            {
                MessageSenders = messageSenders;
                Coordinator = coordinator;
                Message = message;
                Recipients = recipients;
            }

            public List<IMessageSender> MessageSenders { get; set; }
            public MessageCoordinator Coordinator { get; set; }
            public Message Message { get; set; }
            public List<MessageRecipient> Recipients { get; set; }
        }

        private static void InvokeSendMessageMethod(TestObjects testData)
        {
            testData.Coordinator.SendMessage(testData.Message, testData.Recipients);
        }

        private static TestObjects SetupTestObjects(params Mock<IMessageSender>[] fakeSenders)
        {
            var messageSenders = fakeSenders.Select(s => s.Object).ToList();
            return new TestObjects(
                messageSenders,
                new MessageCoordinator(messageSenders),
                new Message("Test subject", "test body"),
                new List<MessageRecipient>());
        }

    }
}