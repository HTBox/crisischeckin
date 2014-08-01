﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using Models;
using Moq;
using Services.Interfaces;

namespace Services.UnitTest
{
    [TestFixture]
    public class MessageServiceTests
    {
        [Test]
        public void MessageService__Calls_MessageCoordinator_WithProperVolunteerList()
        {
            var mockAdminSvc = new Mock<IAdmin>();
            var volunteers = new List<Person>();
            volunteers.Add(new Person { Id = 1, Email = "email-1", FirstName = "first", LastName = "last" });
            volunteers.Add(new Person { Id = 2, Email = "email-2", FirstName = "first", LastName = "last" });
            mockAdminSvc.Setup(x => x.GetVolunteersForDate(It.IsAny<int>(), DateTime.Today)).Returns(volunteers);
            var mockMessageCoordinator = new Mock<IMessageCoordinator>();

            var expectedMessage = new Message("body", "subject");
            var expectedRecipients = new List<MessageRecipient>();
            expectedRecipients.Add(new MessageRecipient { EmailAddress = "email-1", Name = "first last" });
            expectedRecipients.Add(new MessageRecipient { EmailAddress = "email-2", Name = "first last" });

            var sut = new MessageService(mockAdminSvc.Object, mockMessageCoordinator.Object);
            sut.SendMessageToDisasterVolunteers(new RecipientCriterion(42), expectedMessage);

            mockMessageCoordinator.Verify(x => x.SendMessage(expectedMessage, It.Is<List<MessageRecipient>>(y => y.Count.Equals(2))), Times.Once());
        }
    }
}
