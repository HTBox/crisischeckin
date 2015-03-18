using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Models;
using Moq;
using Services.Exceptions;
using Services.Interfaces;

namespace Services.UnitTest
{
    [TestFixture]
    public class VolunteerServiceTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Register_NullFirstName()
        {
            VolunteerService service = new VolunteerService(new Mock<IDataService>().Object);
            service.Register("", "last", "email", "555-333-1111", 1, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Register_NullLastName()
        {
            VolunteerService service = new VolunteerService(new Mock<IDataService>().Object);
            service.Register("first", "", "email", "555-333-1111", 1, 2);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Register_NullEmail()
        {
            VolunteerService service = new VolunteerService(new Mock<IDataService>().Object);
            service.Register("first", "last", "", "555-333-1111", 1, 3);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Register_NullPhoneNumber()
        {
            VolunteerService service = new VolunteerService(new Mock<IDataService>().Object);
            service.Register("first", "last", "email", "", 1, 3);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Register_NullCluster()
        {
            VolunteerService service = new VolunteerService(new Mock<IDataService>().Object);
            service.Register("first", "last", "email", "555-333-1111", 0, 4);
        }

        [Test]
        public void Register_ValidVolunteer()
        {
            Person moqPerson = new Person()
            {
                Id = 1,
                UserId = null,
                FirstName = "Bob",
                LastName = "Jones",
                Email = "bob.jones@email.com",
                PhoneNumber = "555-222-9139",
                ClusterId = 1
            };

            var moqDataService = new Mock<IDataService>();
            moqDataService.Setup(s => s.AddPerson(It.IsAny<Person>())).Returns(moqPerson);

            VolunteerService service = new VolunteerService(moqDataService.Object);
            Person actual = service.Register("Bob", "Jones", "bob.jones@email.com", "555-222-9139", 1, 5);

            Assert.AreEqual(1, actual.Id);
            Assert.AreEqual("Bob", actual.FirstName);
            Assert.AreEqual("Jones", actual.LastName);
            Assert.AreEqual("bob.jones@email.com", actual.Email);
            Assert.AreEqual("555-222-9139", actual.PhoneNumber);
            Assert.AreEqual(1, actual.ClusterId);
        }

        [Test]
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
                PhoneNumber = "555-222-9139",
                ClusterId = 1
            };

            List<Person> people = new List<Person>();
            people.Add(moqPerson);

            var moqDataService = new Mock<IDataService>();
            moqDataService.Setup(s => s.Persons).Returns(people.AsQueryable());

            VolunteerService service = new VolunteerService(moqDataService.Object);
            Person actual = service.Register("Cathy", "Jones", "cathy.jones@email.com", "555-222-9139 ext 33", 1, 6);

            Assert.AreEqual("Cathy", actual.FirstName);
            Assert.AreEqual("Jones", actual.LastName);
            Assert.AreEqual("cathy.jones@email.com", actual.Email);
            Assert.AreEqual("555-222-9139 ext 33", actual.PhoneNumber);
            Assert.AreEqual(1, actual.ClusterId);
        }

		[Test]
		public void Register_UsernameAvailable()
		{
			User moqUser = new User()
			{
				Id = 1,
				UserName = "test123"
			};

			List<User> users = new List<User>();
			users.Add(moqUser);

			var moqDataService = new Mock<IDataService>();
			moqDataService.Setup(s => s.Users).Returns(users.AsQueryable());

			VolunteerService service = new VolunteerService(moqDataService.Object);
			
			//test that the username we created is not available.
			Assert.IsFalse(service.UsernameAvailable("test123"), "Username created for this test should report as not available.");

			//test that a different username is available.
			Assert.IsTrue(service.UsernameAvailable("test456"), "Username that was not added to our data source should report as available.");
		}

        [Test]
        public void Register_EmailAlreadyInUse_ReturnFalse()
        {
            Person moqPerson = new Person()
            {
                Id = 1,
                Email = "matt.smith@email.com"
            };

            List<Person> people = new List<Person>();
            people.Add(moqPerson);

            var moqDataService = new Mock<IDataService>();
            moqDataService.Setup(s => s.Persons).Returns(people.AsQueryable());
            VolunteerService service = new VolunteerService(moqDataService.Object);
            Assert.IsFalse(service.EmailAlreadyInUse("cathy.jones@email.com"));
        }

        [Test]
        public void Register_EmailAlreadyInUse_ReturnTrue()
        {
            Person moqPerson = new Person()
            {
                Id = 1,
                Email = "matt.smith@email.com"
            };

            List<Person> people = new List<Person>();
            people.Add(moqPerson);

            var moqDataService = new Mock<IDataService>();
            moqDataService.Setup(s => s.Persons).Returns(people.AsQueryable());
            VolunteerService service = new VolunteerService(moqDataService.Object);
            Assert.IsTrue(service.EmailAlreadyInUse("matt.smith@email.com"));
        }		

        [Test]
        public void UpdateDetails_Valid()
        {
            Person moqPerson = new Person()
            {
                Id = 1,
                UserId = 6,
                FirstName = "Cathy",
                LastName = "Jones",
                Email = "cathy.jones@email.com",
                PhoneNumber = "555-222-9139",
                ClusterId = 1
            };

            List<Person> people = new List<Person>();
            people.Add(moqPerson);

            var moqDataService = new Mock<IDataService>();
            moqDataService.Setup(s => s.Persons).Returns(people.AsQueryable());
            moqDataService.Setup(s => s.UpdatePerson(It.IsAny<Person>())).Returns(new Person() {
                Id = 1,
                UserId = 6,
                Email = "cathy.jones@email.com",
                FirstName = "Cathy",
                LastName = "CHANGED",
                PhoneNumber = "555-222-9139",
                ClusterId = 1
            });

            VolunteerService service = new VolunteerService(moqDataService.Object);
            var actual = service.UpdateDetails(new Person()
            {
                Id = 1, UserId = 6, Email = "cathy.jones@email.com", FirstName = "Cathy", LastName = "CHANGED", PhoneNumber = "555-222-9139", ClusterId = 1
            });

            // Only Last Name has been updated
            Assert.AreEqual("CHANGED", actual.LastName);

            Assert.AreEqual("Cathy", actual.FirstName);
        }

        [Test]
        [ExpectedException(typeof(PersonNotFoundException))]
        public void UpdateDetails_PersonNotFound()
        {
            Person moqPerson = new Person()
            {
                Id = 1,
                UserId = 6,
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
                Email = "matt.smith@email.com",
                UserId = 2
            });
        }

        [Test]
        [ExpectedException(typeof(PersonEmailAlreadyInUseException))]
        public void UpdateDetails_PersonEmailAlreadyInUse()
        {
            Person moqPersonOne = new Person()
            {
                Id = 1,
                UserId = 6,
                FirstName = "Cathy",
                LastName = "Jones",
                Email = "cathy.jones@email.com",
                PhoneNumber = "555-222-9139"
            };

            Person moqPersonTwo = new Person()
            {
                Id = 2,
                UserId = 7,
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
                UserId = 6,
                Email = "stan.smith@email.com",
                FirstName = "Cathy",
                LastName = "Jones"
            });
        }

        [Test]
        public void UpdateDetails_ChangeEmailToUnusuedEmail_DoesNotThrowPersonEmailAlreadyInUseException()
        {
            Person moqPersonOne = new Person()
            {
                Id = 1,
                UserId = 6,
                FirstName = "Cathy",
                LastName = "Jones",
                Email = "cathy.jones@email.com",
                PhoneNumber = "555-222-9139"
            };

            Person moqPersonTwo = new Person()
            {
                Id = 2,
                UserId = 7,
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
            // Try to change Cathy Jones' email to something not in use...
            service.UpdateDetails(new Person()
            {
                Id = 1,
                UserId = 6,
                Email = "unused@email.com",
                FirstName = "Cathy",
                LastName = "Jones"
            });
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateDetails_NullPerson()
        {
            var moqDataService = new Mock<IDataService>();
            VolunteerService service = new VolunteerService(moqDataService.Object);

            service.UpdateDetails(null);
        }

        [Test]
        public void UpdateDetails_DoesNotClearFirstOrLastNameIfBlank()
        {
            var moqPersonOne = new Person
            {
                Id = 1,
                UserId = 6,
                FirstName = "Cathy",
                LastName = "Jones",
                Email = "cathy.jones@email.com",
                PhoneNumber = "555-222-9139"
            };

            var people = new List<Person> {moqPersonOne};

            var moqDataService = new Mock<IDataService>();
            moqDataService.Setup(s => s.Persons).Returns(people.AsQueryable());
            moqDataService.Setup(s => s.UpdatePerson(It.IsAny<Person>())).Returns((Person p) => p);

            var service = new VolunteerService(moqDataService.Object);

            Person actual = service.UpdateDetails(new Person
            {
                Id = 1,
                UserId = 6,
                FirstName = null,
                LastName = null,
                Email = "cathy.jones@email.com",
                PhoneNumber = "555-222-9139"
            });

            Assert.IsNotNull(actual);
            Assert.IsFalse(string.IsNullOrEmpty(actual.FirstName), "Expected First Name not to be null or empty.");
            Assert.IsFalse(string.IsNullOrEmpty(actual.LastName), "Expected Last Name not to be null or empty.");
        }

        [Test]
        public void WhenPersonIdDoesNotExistReturnsEmptyList()
        {
            var moqDataService = new Mock<IDataService>();
            moqDataService.Setup(service => service.Commitments).Returns(new List<Commitment>().AsQueryable);
            var underTest = new VolunteerService(moqDataService.Object);

            var results = underTest.RetrieveCommitments(0, false);
        }

        [Test]
        public void WhenQueriedAllCommitmentsAreReturned()
        {
            var moqDataService = new Mock<IDataService>();
            var disaster = new Disaster
            {
                Id = 1,
                Name = "test",
                IsActive = true
            };
            moqDataService.Setup(ds => ds.Disasters)
                .Returns(new List<Disaster>
                {
                    disaster
                }.AsQueryable());

            moqDataService.Setup(ds => ds.Commitments)
                .Returns(new List<Commitment>
                {
                    new Commitment
                    {
                        DisasterId=1,
                        Id = 1,
                        PersonId=1,
                        StartDate=new DateTime(2013, 8, 1),
                        EndDate = new DateTime(2013, 9, 1),
                        Disaster=disaster
                    }
                }.AsQueryable());
            var underTest = new VolunteerService(moqDataService.Object);

            const int personId = 1;
            var results = underTest.RetrieveCommitments(personId, false);
            Assert.IsTrue(results.Count() == 1);
            Assert.IsTrue(results.Single().Id == personId);
        }

        [Test]
        public void WhenNoCommitmentsExistAnywhereEmptyCollectionIsReturned()
        {
            var moqDataService = new Mock<IDataService>();
            moqDataService.Setup(ds => ds.Commitments)
                .Returns(new List<Commitment>().AsQueryable());
            var underTest = new VolunteerService(moqDataService.Object);

            var disaster = new Disaster
            {
                Id = 1,
                Name = "test",
                IsActive = true
            };
            moqDataService.Setup(ds => ds.Disasters)
                .Returns(new List<Disaster>
                {
                    disaster
                }.AsQueryable());
            const int personId = 1;

            var results = underTest.RetrieveCommitments(personId, false);
            Assert.IsTrue(!results.Any());
        }

        // Inactive disaster depends on flag
        [Test]
        public void WhenQueryingInactiveDisastersAllCommitmentsReturned()
        {
            var moqDataService = new Mock<IDataService>();
            var disaster = new Disaster
            {
                Id = 1,
                Name = "test",
                IsActive = false
            };
            moqDataService.Setup(ds => ds.Disasters)
                .Returns(new List<Disaster>
                {
                    disaster
                }.AsQueryable());

            moqDataService.Setup(ds => ds.Commitments)
                .Returns(new List<Commitment>
                {
                    new Commitment
                    {
                        DisasterId=1,
                        Id = 1,
                        PersonId=1,
                        StartDate=new DateTime(2013, 8, 1),
                        EndDate = new DateTime(2013, 9, 1),
                        Disaster=disaster
                    }
                }.AsQueryable());
            var underTest = new VolunteerService(moqDataService.Object);

            const int personId = 1;
            var results = underTest.RetrieveCommitments(personId, true);
            Assert.IsTrue(results.Count() == 1);
            Assert.IsTrue(results.Single().Id == 1);
        }

        [Test]
        public void WhenQueryingActiveDisastersFilteredCommitmentsReturned()
        {
            var moqDataService = new Mock<IDataService>();
            var disaster = new Disaster
            {
                Id = 1,
                Name = "test",
                IsActive = false
            };
            moqDataService.Setup(ds => ds.Disasters)
                .Returns(new List<Disaster>
                {
                    disaster
                }.AsQueryable());

            moqDataService.Setup(ds => ds.Commitments)
                .Returns(new List<Commitment>
                {
                    new Commitment
                    {
                        DisasterId=1,
                        Id = 1,
                        PersonId=1,
                        StartDate=new DateTime(2013, 8, 1),
                        EndDate = new DateTime(2013, 9, 1),
                        Disaster=disaster
                    }
                }.AsQueryable());
            var underTest = new VolunteerService(moqDataService.Object);

            var personId = 1;
            var results = underTest.RetrieveCommitments(personId, false);
            Assert.IsTrue(!results.Any());
        }

        // only records for this user
        [Test]
        public void WhenQueryingReturnCommitmentsForThisUser()
        {
            var moqDataService = new Mock<IDataService>();
            var disaster = new Disaster
            {
                Id = 1,
                Name = "test",
                IsActive = true
            };
            moqDataService.Setup(ds => ds.Disasters)
                .Returns(new List<Disaster>
                {
                    disaster
                }.AsQueryable());

            moqDataService.Setup(ds => ds.Commitments)
                .Returns(new List<Commitment>
                {
                    new Commitment
                    {
                        DisasterId=1,
                        Id = 1,
                        PersonId=1,
                        StartDate=new DateTime(2013, 8, 1),
                        EndDate = new DateTime(2013, 9, 1),
                        Disaster=disaster
                    },
                    new Commitment
                    {
                        DisasterId=1,
                        Id = 2,
                        PersonId=2,
                        StartDate=new DateTime(2013, 8, 1),
                        EndDate = new DateTime(2013, 9, 1),
                        Disaster=disaster
                    }
                }.AsQueryable());
            var underTest = new VolunteerService(moqDataService.Object);

            const int personId = 1;
            var results = underTest.RetrieveCommitments(personId, false);
            Assert.IsTrue(results.Count() == 1);
        }


        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
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

            var results = underTest.RetrieveCommitmentsForDisaster(person, default(Disaster));
        }

        [Test]
        public void WhenQueriedActiveCommitmentsAreReturned()
        {
            var moqDataService = new Mock<IDataService>();
            var disaster = new Disaster
            {
                Id = 1,
                Name = "test",
                IsActive = true
            };
            moqDataService.Setup(ds => ds.Disasters)
                .Returns(new List<Disaster>
                {
                    disaster
                }.AsQueryable());

            moqDataService.Setup(ds => ds.Commitments)
                .Returns(new List<Commitment>
                {
                    new Commitment
                    {
                        DisasterId=1,
                        Id = 1,
                        PersonId=1,
                        StartDate=new DateTime(2013, 8, 1),
                        EndDate = new DateTime(2013, 9, 1),
                        Disaster=disaster
                    }
                }.AsQueryable());
            var underTest = new VolunteerService(moqDataService.Object);

            var person = new Person
            {
                Id = 1,
                FirstName = "test",
                LastName = "tester"
            };

            var results = underTest.RetrieveCommitmentsForDisaster(person, disaster);
            Assert.IsTrue(results.Count() == 1);
            Assert.IsTrue(results.Single().Id == 1);
        }

        [Test]
        public void WhenNoCommitmentsExistEmptyCollectionIsReturned()
        {
            var moqDataService = new Mock<IDataService>();
            moqDataService.Setup(ds => ds.Commitments)
                .Returns(new List<Commitment>().AsQueryable());
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
            moqDataService.Setup(ds => ds.Disasters)
                .Returns(new List<Disaster>
                {
                    disaster
                }.AsQueryable());

            var results = underTest.RetrieveCommitmentsForDisaster(person, disaster);
            Assert.IsTrue(results.Count() == 0);
        }

        // Inactive disaster depends on flag
        [Test]
        public void WhenQueryingDisasterAllCommitmentsReturned()
        {
            var moqDataService = new Mock<IDataService>();
            var disaster = new Disaster
            {
                Id = 1,
                Name = "test",
                IsActive = false
            };
            moqDataService.Setup(ds => ds.Disasters)
                .Returns(new List<Disaster>
                {
                    disaster
                }.AsQueryable());

            moqDataService.Setup(ds => ds.Commitments)
                .Returns(new List<Commitment>
                {
                    new Commitment
                    {
                        DisasterId=1,
                        Id = 1,
                        PersonId=1,
                        StartDate=new DateTime(2013, 8, 1),
                        EndDate = new DateTime(2013, 9, 1),
                        Disaster=disaster
                    }
                }.AsQueryable());
            var underTest = new VolunteerService(moqDataService.Object);

            var person = new Person
            {
                Id = 1,
                FirstName = "test",
                LastName = "tester"
            };
            var results = underTest.RetrieveCommitmentsForDisaster(person, disaster);
            Assert.IsTrue(results.Count() == 1);
            Assert.IsTrue(results.Single().Id == 1);
        }

        // only records for this user
        [Test]
        public void WhenQueryingReturnCommitmentsOnlyForThisUser()
        {
            var moqDataService = new Mock<IDataService>();
            var disaster = new Disaster
            {
                Id = 1,
                Name = "test",
                IsActive = true
            };
            moqDataService.Setup(ds => ds.Disasters)
                .Returns(new List<Disaster>
                {
                    disaster
                }.AsQueryable());

            moqDataService.Setup(ds => ds.Commitments)
                .Returns(new List<Commitment>
                {
                    new Commitment
                    {
                        DisasterId=1,
                        Id = 1,
                        PersonId=1,
                        StartDate=new DateTime(2013, 8, 1),
                        EndDate = new DateTime(2013, 9, 1),
                        Disaster=disaster
                    },
                    new Commitment
                    {
                        DisasterId=1,
                        Id = 2,
                        PersonId=2,
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
            var results = underTest.RetrieveCommitmentsForDisaster(person, disaster);
            Assert.IsTrue(results.Count() == 1);
        }

        // Only records for the specific disaster
        [Test]
        public void WhenQueryingReturnCommitmentsOnlyForThisDisaster()
        {
            var moqDataService = new Mock<IDataService>();
            var disaster = new Disaster
            {
                Id = 1,
                Name = "test",
                IsActive = true
            };
            var disaster2 = new Disaster
            {
                Id = 2,
                Name = "test",
                IsActive = true
            };

            moqDataService.Setup(ds => ds.Disasters)
                .Returns(new List<Disaster>
                {
                    disaster,
                    disaster2
                }.AsQueryable());

            moqDataService.Setup(ds => ds.Commitments)
                .Returns(new List<Commitment>
                {
                    new Commitment
                    {
                        DisasterId=1,
                        Id = 1,
                        PersonId=1,
                        StartDate=new DateTime(2013, 8, 1),
                        EndDate = new DateTime(2013, 9, 1),
                        Disaster=disaster
                    },
                    new Commitment
                    {
                        DisasterId=2,
                        Id = 2,
                        PersonId=1,
                        StartDate=new DateTime(2013, 8, 1),
                        EndDate = new DateTime(2013, 9, 1),
                        Disaster=disaster2
                    }
                }.AsQueryable());
            var underTest = new VolunteerService(moqDataService.Object);

            var person = new Person
            {
                Id = 1,
                FirstName = "test",
                LastName = "tester"
            };

            var results = underTest.RetrieveCommitmentsForDisaster(person, disaster);
            Assert.AreEqual(1, results.Count());
        }

        [Test]
        public void GetPersonDetailsForChangeContactInfoReturnsExpectedData()
        {
            var personOne = new Person
            {
                Id = 1,
                UserId = 6,
                FirstName = "Cathy",
                LastName = "Jones",
                Email = "cathy.jones@email.com",
                PhoneNumber = "555-222-9139",
                ClusterId = 1
            };

            var personTwo = new Person
            {
                Id = 2,
                UserId = 7,
                FirstName = "Stan",
                LastName = "Smith",
                Email = "stan.smith@email.com",
                PhoneNumber = "111-333-2222"
            };

            var dataService = new Mock<IDataService>();
            var personList = new List<Person>
            {
                personOne,
                personTwo
            };
            dataService.Setup(x => x.Persons).Returns(personList.AsQueryable());
            var underTest = new VolunteerService(dataService.Object);

            var result = underTest.GetPersonDetailsForChangeContactInfo(personTwo.UserId.GetValueOrDefault());

            Assert.IsNotNull(result);
            Assert.AreEqual(personTwo.Email, result.Email);
            Assert.AreEqual(personTwo.PhoneNumber, result.PhoneNumber);
        }

        [Test]
        [ExpectedException(typeof (PersonNotFoundException))]
        public void GetPersonDetailsForChangeContactInfoThrowsExpectedPersonNotFoundException()
        {
            var personOne = new Person
            {
                Id = 1,
                UserId = 6,
                FirstName = "Cathy",
                LastName = "Jones",
                Email = "cathy.jones@email.com",
                PhoneNumber = "555-222-9139",
                ClusterId = 1
            };

            var personTwo = new Person
            {
                Id = 2,
                UserId = 7,
                FirstName = "Stan",
                LastName = "Smith",
                Email = "stan.smith@email.com",
                PhoneNumber = "111-333-2222"
            };

            var dataService = new Mock<IDataService>();
            var personList = new List<Person>
            {
                personOne,
                personTwo
            };
            dataService.Setup(x => x.Persons).Returns(personList.AsQueryable());
            var underTest = new VolunteerService(dataService.Object);

            var result = underTest.GetPersonDetailsForChangeContactInfo(8);
        }
    }
}
