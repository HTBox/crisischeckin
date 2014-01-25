using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;
using Services.Interfaces;

namespace Services.UnitTest
{
    [TestClass]
    public class MessageServiceTests
    {
        [TestMethod]
        public void MessageService__Calls_MessageCoordinator_WithProperVolunteerList()
        {
            var mockAdminSvc = new Mock<IAdmin>();
            var volunteers = new List<Person>();
            volunteers.Add(new Person { Id = 1, PhoneNumber = "phone-number-1", Email = "email-1" });
            volunteers.Add(new Person { Id = 2, PhoneNumber = "phone-number-2", Email = "email-2" });
            mockAdminSvc.Setup(x => x.GetVolunteersForDate(It.IsAny<int>(), DateTime.Today)).Returns(volunteers);
            //var mockMessageCoordinator = new Mock<IMessageCoordinator>();

        }
    }
}
