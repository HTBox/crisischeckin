using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using System.Linq;
using Moq;
using Models;
using System.Collections.Generic;
using Services.Interfaces;

namespace WebProjectTests.ServiceTests
{
    [TestClass]
    public class AdminServiceTests
    {
        private const int disasterWithVolunteersID = 1;
        private const int disasterWithNoVolunteersID = 2;

        private const int personWithCommitmentsID = 11;
        private const int personWithNoCommitmentsID = 12;

        private const int commitmentId = 101;

        private readonly Disaster disasterWithCommitments = new Disaster
        {
            Id = disasterWithVolunteersID,
            Name = "Post Conference party cleanup"
        };

        private readonly Disaster disasterWithNoCommitments = new Disaster
        {
            Id = disasterWithNoVolunteersID,
            Name = "Post Conference security deposit grovelling"
        };

        private readonly Person personWithCommitments = new Person
        {
            Id = personWithCommitmentsID,
            FirstName = "Richard",
            LastName = "Campbell",
            PhoneNumber = "(111) 555-1212",
            Email = "unused@nothere.com"
        };

        private readonly Person personWithNoCommitments = new Person
        {
            Id = personWithNoCommitmentsID,
            FirstName = "Lazy",
            LastName = "Volunteer",
            PhoneNumber = "(111) 555-1414",
            Email = "DontCallMe@nothere.com"
        };
        
        private readonly Commitment commitment = new Commitment
                            {
                                DisasterId = disasterWithVolunteersID,
                                Id = commitmentId,
                                PersonId = personWithCommitmentsID,
                                StartDate = new DateTime(2013, 8, 10),
                                EndDate = new DateTime(2013, 8, 15)
                                };
        
        private Mock<IDataService> mockService;

        [TestInitialize]
        public void CreateDependencies()
        {
            mockService = new Mock<IDataService>();
        }

        [TestMethod,
        ExpectedException(typeof(ArgumentNullException))]
        public void WhenServiceIsNullConstructorThrowsExceptions()
        {
            var underTest = new AdminService(default(IDataService));
        }

        [TestMethod]
        public void WhenOneVolunteerHasRegisteredReturnThatOneRecord()
        {
            initializeDisasterCollection(disasterWithCommitments);
            initializeVolunteerCollection(personWithCommitments);
            initializeCommitmentCollection(commitment);

            var underTest = new AdminService(mockService.Object);

            var result = underTest.GetVolunteers(disasterWithCommitments);

            Assert.AreEqual(1, result.Count());
        }

        //with empty collections, return an empty list.
        [TestMethod]
        public void WhenNoVolunteersAreRegisteredReturnAnEmptyList()
        {
            var underTest = new AdminService(mockService.Object);

            initializeDisasterCollection(disasterWithNoCommitments);

            var result = underTest.GetVolunteers(disasterWithNoCommitments);
            Assert.IsFalse(result.Any());
        }

        [TestMethod,
        ExpectedException(typeof(ArgumentNullException))]
        public void WhenDisasterIsNullThrowNullArgumentException()
        {
            var underTest = new AdminService(mockService.Object);
            var result = underTest.GetVolunteers(default(Disaster));
        }

        [TestMethod,
        ExpectedException(typeof(ArgumentException))]
        public void WhenDisasterIsNotFoundThrowArgumentException()
        {
            var underTst = new AdminService(mockService.Object);

            initializeDisasterCollection(disasterWithCommitments);
            initializeVolunteerCollection(personWithCommitments);
            initializeCommitmentCollection(commitment);

            var disaster = new Disaster
            {
                Id = -1,
                Name = "DoesNotExist"
            };
            underTst.GetVolunteers(disaster);
        }

        [TestMethod]
        public void WhenVolunteerDontCommitReturnCommittedRecords()
        {
            initializeDisasterCollection(disasterWithCommitments);
            initializeVolunteerCollection(personWithCommitments, personWithNoCommitments);
            initializeCommitmentCollection(commitment);

            var underTest = new AdminService(mockService.Object);

            var result = underTest.GetVolunteers(disasterWithCommitments);

            Assert.AreEqual(1, result.Count());
        }


        // person with multiple commitments only once
        [TestMethod]
        public void WhenOneVolunteerHasMultipleRegistrationsReturnExactlyOneRecord()
        {
            initializeDisasterCollection(disasterWithCommitments);
            initializeVolunteerCollection(personWithCommitments);

            var secondCommitment = new Commitment
            {
                DisasterId = disasterWithVolunteersID,
                Id = 102,
                PersonId = personWithNoCommitmentsID,
                StartDate = new DateTime(2013, 8, 15),
                EndDate = new DateTime(2013, 8, 20)
            };
            initializeCommitmentCollection(commitment, secondCommitment);

            var underTest = new AdminService(mockService.Object);

            var result = underTest.GetVolunteers(disasterWithCommitments);

            Assert.AreEqual(1, result.Count());
        }

        // people with commitments on other disasters does not show up
        // person with multiple commitments only once
        [TestMethod]
        public void WhenVolunteersExistForMultipleDisastersReturnCorrectDisaster()
        {
            initializeDisasterCollection(disasterWithCommitments, disasterWithNoCommitments);
            initializeVolunteerCollection(personWithCommitments, personWithNoCommitments);

            var secondCommitment = new Commitment
            {
                DisasterId = disasterWithNoVolunteersID,
                Id = 102,
                PersonId = personWithNoCommitmentsID,
                StartDate = new DateTime(2013, 8, 15),
                EndDate = new DateTime(2013, 8, 20)
            };
            initializeCommitmentCollection(commitment, secondCommitment);

            var underTest = new AdminService(mockService.Object);

            var result = underTest.GetVolunteers(disasterWithCommitments);

            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void WhenOneVolunteerHasRegisteredReturnOneRecordInRange()
        {
            initializeDisasterCollection(disasterWithCommitments);
            initializeVolunteerCollection(personWithCommitments);
            initializeCommitmentCollection(commitment);

            var underTest = new AdminService(mockService.Object);

            var result = underTest.GetVolunteersForDate(disasterWithCommitments, new DateTime(2013,08,12));

            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void WhenOneVolunteerHasRegisteredReturnNoRecordsInRange()
        {
            initializeDisasterCollection(disasterWithCommitments);
            initializeVolunteerCollection(personWithCommitments);
            initializeCommitmentCollection(commitment);

            var underTest = new AdminService(mockService.Object);

            var result = underTest.GetVolunteersForDate(disasterWithCommitments, new DateTime(2013, 08, 5));

            Assert.AreEqual(0, result.Count());
        }

        private void initializeDisasterCollection(params Disaster[] disasters)
        {
            mockService.Setup(ds => ds.Disasters).Returns(disasters.AsQueryable());
        }

        private void initializeVolunteerCollection(params Person [] people)
        {
            mockService.Setup(ds => ds.Persons).Returns(people.AsQueryable());  
        }

        private void initializeCommitmentCollection(params Commitment[] commitments)
        {
            mockService.Setup(ds => ds.Commitments).Returns(commitments.AsQueryable());
        }
    }
}
