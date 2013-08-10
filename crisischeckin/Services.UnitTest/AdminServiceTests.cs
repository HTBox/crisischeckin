using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using System.Linq;
using Moq;
using Models;
using System.Collections.Generic;

namespace WebProjectTests.ServiceTests
{
    [TestClass]
    public class AdminServiceTests
    {
        private const int disasterWithVolunteersID = 1;
        private const int disasterWithNoVolunteersID = 2;
        private const int personWithCommitmentsID = 3;
        private const int commitmentId = 4;

        private readonly Disaster disasterWithCommitments = new Disaster
        {
            Id = disasterWithVolunteersID,
            Name = "Post Conference party cleanup"
        };

        private readonly Disaster disasterWithNoCommitments = new Disaster
        {
            Id = disasterWithVolunteersID,
            Name = "Post Conference party cleanup"
        };

        private readonly Person personWithCommitments = new Person
        {
            Id = personWithCommitmentsID,
            FirstName = "Richard",
            LastName = "Campbell",
            PhoneNumber = "(111) 555-1212",
            Email = "unused@nothere.com"
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

        [TestMethod,
        ExpectedException(typeof(ArgumentException))]
        public void WhenOneVolunteerHasRegisteredReturnThatOneRecord()
        {
            initializeDisasterCollection(disasterWithCommitments);
            initializeVolunteerCollection(personWithCommitments);
            initializeCommitmentCollection(commitment);

            var missingDisaster = new Disaster
            {
                Id = -1
            };

            var underTest = new AdminService(mockService.Object);

            var result = underTest.GetVolunteers(missingDisaster);

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

        [TestMethod]
        public void WhenDisasterIsNotFoundThrowArgumentException()
        {
            var underTst = new AdminService(mockService.Object);

            initializeDisasterCollection(disasterWithCommitments);
            initializeVolunteerCollection(personWithCommitments);
            initializeCommitmentCollection(commitment);
        }

        private void initializeDisasterCollection(params Disaster [] disasters)
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
