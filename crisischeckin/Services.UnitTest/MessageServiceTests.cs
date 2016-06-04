﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            const string sender = "Sender Name";

            var mockAdminSvc = new Mock<IAdmin>();
            var volunteers = new List<Person>();
            volunteers.Add(new Person { Id = 1, Email = "email-1", FirstName = "first", LastName = "last" });
            volunteers.Add(new Person { Id = 2, Email = "email-2", FirstName = "first", LastName = "last" });
            mockAdminSvc.Setup(x => x.GetVolunteersForDate(It.IsAny<int>(), DateTime.Today, false, false,null)).Returns(volunteers);
            var mockMessageCoordinator = new Mock<IMessageCoordinator>();

            var expectedMessage = new Message("body", "subject");
            var expectedRecipients = new List<MessageRecipient>();
            expectedRecipients.Add(new MessageRecipient { EmailAddress = "email-1", Name = "first last" });
            expectedRecipients.Add(new MessageRecipient { EmailAddress = "email-2", Name = "first last" });

            var sut = new MessageService(mockAdminSvc.Object, mockMessageCoordinator.Object);
            sut.SendMessageToDisasterVolunteers(expectedMessage, new RecipientCriterion(42), sender);

            mockMessageCoordinator.Verify(x => x.SendMessage(expectedMessage, It.Is<List<MessageRecipient>>(y => y.Count.Equals(2)), sender), Times.Once());
        }

        [Test]
        public void MessageService__DoesNotFail_With_Empty_VolunteerList()
        {
            const string sender = "Sender Name";

            var mockAdminSvc = new Mock<IAdmin>();
            var volunteers = new List<Person>();
            volunteers.Add(new Person { Id = 1, Email = "email-1", FirstName = "first", LastName = "last" });
            volunteers.Add(new Person { Id = 2, Email = "email-2", FirstName = "first", LastName = "last" });
            mockAdminSvc.Setup(x => x.GetVolunteersForDate(It.IsAny<int>(), DateTime.Today, false, false,null)).Returns(volunteers);
            var mockMessageCoordinator = new Mock<IMessageCoordinator>();

            var expectedMessage = new Message("body", "subject");
            var expectedRecipients = new List<MessageRecipient>();

            var sut = new MessageService(mockAdminSvc.Object, mockMessageCoordinator.Object);
            sut.SendMessageToDisasterVolunteers(expectedMessage, new RecipientCriterion(42), sender);

            mockMessageCoordinator.Verify(x => x.SendMessage(expectedMessage, It.Is<List<MessageRecipient>>(y => y.Count.Equals(2)), sender), Times.Once());
        }

        [Test]
        public void MessageService__Calls_AdminSvc_WithSelectedClusters()
        {
            var mockAdminSvc = new Mock<IAdmin>();
            var mockMessageCoordinator = new Mock<IMessageCoordinator>();

            var recipientCriterion = new RecipientCriterion(1, new []{1,2}, false,false);

            var sut = new MessageService(mockAdminSvc.Object, mockMessageCoordinator.Object);
            sut.SendMessageToDisasterVolunteers(new Message("Body","Subject"), recipientCriterion );

            mockAdminSvc.Verify(m=>m.GetVolunteersForDate(1,DateTime.Today,false,false, recipientCriterion.ClusterIds), Times.Once());

        }
    }
}
