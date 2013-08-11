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
            VolunteerService service = new VolunteerService(new Mock<IDataService>().Object);
            service.Register("", "last", "email", "555-333-1111");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Register_NullLastName()
        {
            VolunteerService service = new VolunteerService(new Mock<IDataService>().Object);
            service.Register("first", "", "email", "555-333-1111");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Register_NullEmail()
        {
            VolunteerService service = new VolunteerService(new Mock<IDataService>().Object);
            service.Register("first", "last", "", "555-333-1111");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Register_NullPhoneNumber()
        {
            VolunteerService service = new VolunteerService(new Mock<IDataService>().Object);
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

        [TestMethod]
        public void UpdateDetails_Valid()
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
            moqDataService.Setup(s => s.UpdatePerson(It.IsAny<Person>())).Returns(new Person() {
                Id = 1,
                Email = "cathy.jones@email.com",
                FirstName = "Cathy",
                LastName = "CHANGED",
                PhoneNumber = "555-222-9139"
            });

            VolunteerService service = new VolunteerService(moqDataService.Object);
            var actual = service.UpdateDetails(new Person()
            {
                Id = 1, Email = "cathy.jones@email.com", FirstName = "Cathy", LastName = "CHANGED", PhoneNumber = "555-222-9139"
            });

            // Only Last Name has been updated
            Assert.AreEqual("CHANGED", actual.LastName);

            Assert.AreEqual("Cathy", actual.FirstName);
        }

        [TestMethod]
        [ExpectedException(typeof(PersonNotFoundException))]
        public void UpdateDetails_PersonNotFound()
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

            service.UpdateDetails(new Person()
            {
                Id = 25,
                Email = "matt.smith@email.com"
            });
        }

        [TestMethod]
        [ExpectedException(typeof(PersonEmailAlreadyInUseException))]
        public void UpdateDetails_PersonEmailAlreadyInUse()
        {
            Person moqPersonOne = new Person()
            {
                Id = 1,
                UserId = null,
                FirstName = "Cathy",
                LastName = "Jones",
                Email = "cathy.jones@email.com",
                PhoneNumber = "555-222-9139"
            };

            Person moqPersonTwo = new Person()
            {
                Id = 2,
                UserId = null,
                FirstName = "Stan",
                LastName = "Smith",
                Email = "stan.smith@email.com",
                PhoneNumber = "111-333-2222"
            };

            List<Person> people = new List<Person>();
            people.Add(moqPersonOne);
            people.Add(moqPersonTwo);

            var moqDataService = new Mock<IDataService>();
            moqDataService.Setup(s => s.Persons).Returns(people.AsQueryable());

            VolunteerService service = new VolunteerService(moqDataService.Object);
            // Try to change Cathy Jones' email to Stan Smith's email...
            service.UpdateDetails(new Person()
            {
                Id = 1,
                Email = "stan.smith@email.com",
                FirstName = "Cathy",
                LastName = "Jones"
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateDetails_NullPerson()
        {
            var moqDataService = new Mock<IDataService>();
            VolunteerService service = new VolunteerService(moqDataService.Object);

            service.UpdateDetails(null);
        }

        [TestMethod,
        ExpectedException(typeof(ArgumentNullException))]
        public void WhenUserIsNullGetCommitmentsThrowsNullArgumentException()
        {
            var moqDataService = new Mock<IDataService>();
            var underTest = new VolunteerService(moqDataService.Object);

            var disaster = new Disaster
            {
                Id = 1,
                Name = "test",
                IsActive = true
            };

            var results = underTest.RetrieveCommitmentsForDisaster(default(Person), disaster, false);
        }

        [TestMethod,
        ExpectedException(typeof(ArgumentNullException))]
        public void WhenDisasterIsNullGetCommitmentsThrowsNullArgumentException()
        {
            var moqDataService = new Mock<IDataService>();
            var underTest = new VolunteerService(moqDataService.Object);

            var person = new Person
            {
                Id = 1,
                FirstName = "test",
                LastName = "tester"
            };

            var results = underTest.RetrieveCommitmentsForDisaster(person, default(Disaster), false);
        }


        [TestMethod]
        public void WhenQueriedActiveCommitmentsAreReturned()
        {
            var moqDataService = new Mock<IDataService>();
            moqDataService.Setup(ds => ds.Commitments)
                .Returns(new List<Commitment>
                {
                    new Commitment
                    {
                        DisasterId=1,
                        Id = 1,
                        PersonId=1,
                        StartDate=new DateTime(2013, 8, 1),
                        EndDate = new DateTime(2013, 9, 1)
                    }
                }.AsQueryable());
            var underTest = new VolunteerService(moqDataService.Object);

            var person = new Person
            {
                Id = 1,
                FirstName = "test",
                LastName = "tester"
            };
            var disaster = new Disaster
            {
                Id = 1,
                Name = "test",
                IsActive = true
            };

            var results = underTest.RetrieveCommitmentsForDisaster(person, disaster, false);
            Assert.IsTrue(results.Count() == 1);
            Assert.IsTrue(results.Single().Id == 1);
        }

        // No commitments returns no records:

        // Inactive disaster depends on flag

        // only records for this user
    }
}
