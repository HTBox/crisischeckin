using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.Interfaces;

namespace Services.UnitTest
{
    [TestClass]
    public class VolunteerServiceTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Register_NullFirstName()
        {
            VolunteerService service = new VolunteerService();
            service.Register("", "last", "email", "555-333-1111");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Register_NullLastName()
        {
            VolunteerService service = new VolunteerService();
            service.Register("first", "", "email", "555-333-1111");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Register_NullEmail()
        {
            VolunteerService service = new VolunteerService();
            service.Register("first", "last", "", "555-333-1111");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Register_NullPhoneNumber()
        {
            VolunteerService service = new VolunteerService();
            service.Register("first", "last", "email", "");
        }

        [TestMethod]
        public void Register_ValidVolunteer()
        {
            VolunteerService service = new VolunteerService();
            bool actual = service.Register("Bob", "Jones", "bob.jones@email.com", "555-222-9139");

            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void Register_ValidVolunteerTwo()
        {
            VolunteerService service = new VolunteerService();
            bool actual = service.Register("Cathy", "Jones", "cathy.jones@email.com", "555-222-9139 ext 33");

            Assert.AreEqual(true, actual);
        }

    }
}
