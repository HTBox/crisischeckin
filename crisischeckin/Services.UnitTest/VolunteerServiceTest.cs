using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;
using Services.Exceptions;
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
            Person moqPerson = new Person()
            {
                Id = 1,
                UserId = null,
                FirstName = "Bob",
                LastName = "Jones",
                Email = "bob.jones@email.com",
                PhoneNumber = "555-222-9139"
            };

            var moqDataService = new Mock<IDataService>();
            moqDataService.Setup(s => s.AddPerson(It.IsAny<Person>())).Returns(moqPerson);

            VolunteerService service = new VolunteerService(moqDataService.Object);
            Person actual = service.Register("Bob", "Jones", "bob.jones@email.com", "555-222-9139");

            Assert.AreEqual(1, actual.Id);
            Assert.AreEqual("Bob", actual.FirstName);
            Assert.AreEqual("Jones", actual.LastName);
            Assert.AreEqual("bob.jones@email.com", actual.Email);
            Assert.AreEqual("555-222-9139", actual.PhoneNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(PersonAlreadyExistsException))]
        public void Register_VolunteerAlreadyExists()
        {
            Person moqPerson = new Person()
            {
                Id = 1,
                UserId = null,
                FirstName = "Cathy",
                LastName = "Jones",
                Email = "cathy.jones@email.com",
                PhoneNumber = "555-222-9139"
            };

            List<Person> people = new List<Person>();
            people.Add(moqPerson);

            var moqDataService = new Mock<IDataService>();
            moqDataService.Setup(s => s.Persons).Returns(people.AsQueryable());

            VolunteerService service = new VolunteerService(moqDataService.Object);
            Person actual = service.Register("Cathy", "Jones", "cathy.jones@email.com", "555-222-9139 ext 33");

            Assert.AreEqual("Cathy", actual.FirstName);
            Assert.AreEqual("Jones", actual.LastName);
            Assert.AreEqual("cathy.jones@email.com", actual.Email);
            Assert.AreEqual("555-222-9139 ext 33", actual.PhoneNumber);
        }

    }
}
