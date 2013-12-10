using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;

namespace Services.UnitTest
{
    [TestClass]
    public class DisasterServiceTest
    {

        private readonly Disaster _activeDisaster = new Disaster
        {
            Id = 1,
            Name = "Test Disaster",
            IsActive = true
        };

        private readonly Disaster _inActiveDisaster = new Disaster
        {
            Id = 2,
            Name = "Test Disaster 2",
            IsActive = false
        };

        private Mock<IDataService> _mockService;

        [TestInitialize]
        public void CreateDependencies()
        {
            _mockService = new Mock<IDataService>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullDataService()
        {
            DisasterService service = new DisasterService(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AssignToVolunteer_NullDisaster()
        {
            var moqDataService = new Mock<IDataService>();
            DisasterService service = new DisasterService(moqDataService.Object);

            service.AssignToVolunteer(null, new Person(), new DateTime(2014, 01, 01), new DateTime(2014, 02, 02));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AssignToVolunteer_NullPerson()
        {
            var moqDataService = new Mock<IDataService>();
            DisasterService service = new DisasterService(moqDataService.Object);

            service.AssignToVolunteer(new Disaster(), null, new DateTime(2014, 01, 01), new DateTime(2014, 02, 02));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AssignToVolunteer_BeginDateGreaterThanEndDate()
        {
            var moqDataService = new Mock<IDataService>();
            DisasterService service = new DisasterService(moqDataService.Object);

            service.AssignToVolunteer(new Disaster(), new Person(), new DateTime(2013, 6, 13), new DateTime(2013, 5, 10));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AssignToVolunteer_StartDateIsWithinExistingCommitment()
        {
            var moqDataService = new Mock<IDataService>();
            DisasterService service = new DisasterService(moqDataService.Object);

            var commitments = new List<Commitment>() { 
                new Commitment() {
                    StartDate = new DateTime(2013, 6, 10),
                    EndDate = new DateTime(2013, 6, 15),
                    DisasterId = 2
                }
            };
            var disasters = new List<Disaster>() { new Disaster() { Id = 2, IsActive = true } };

            moqDataService.Setup(s => s.Commitments).Returns(commitments.AsQueryable());
            moqDataService.Setup(s => s.Disasters).Returns(disasters.AsQueryable());

            service.AssignToVolunteer(new Disaster(), new Person(), new DateTime(2013, 6, 11), new DateTime(2013, 6, 20));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AssignToVolunteer_EndDateIsWithinExistingCommitment()
        {
            var moqDataService = new Mock<IDataService>();
            DisasterService service = new DisasterService(moqDataService.Object);

            var commitments = new List<Commitment>() { 
                new Commitment() {
                    StartDate = new DateTime(2013, 6, 10),
                    EndDate = new DateTime(2013, 6, 15),
                    DisasterId = 2
                }
            };
            var disasters = new List<Disaster>() { new Disaster() { Id = 2, IsActive = true } };

            moqDataService.Setup(s => s.Commitments).Returns(commitments.AsQueryable());
            moqDataService.Setup(s => s.Disasters).Returns(disasters.AsQueryable());

            service.AssignToVolunteer(new Disaster(), new Person(), new DateTime(2013, 6, 5), new DateTime(2013, 6, 12));
        }

        [TestMethod]
        public void AssignToVolunteer_Valid()
        {
            var moqDataService = new Mock<IDataService>();

            Commitment createdCommitment = null;
            moqDataService.Setup(s => s.AddCommitment(It.IsAny<Commitment>()))
                .Callback<Commitment>(commitment => createdCommitment = commitment);
            var service = new DisasterService(moqDataService.Object);

            var person = new Person {Id = 5, Email = "bob.jones@email.com"};
            var disaster = new Disaster {Id = 10, Name = "A disaster"};
            var startDate = new DateTime(2013, 01, 01);
            var endDate = new DateTime(2013, 02, 01);
            service.AssignToVolunteer(disaster, person, startDate, endDate);

            Assert.IsNotNull(createdCommitment);
            Assert.AreEqual(createdCommitment.PersonId, person.Id);
            Assert.AreEqual(createdCommitment.DisasterId, disaster.Id);
            Assert.AreEqual(createdCommitment.StartDate, startDate);
            Assert.AreEqual(createdCommitment.EndDate, endDate);
        }

        [TestMethod]
        public void CreateDisaster_Valid()
        {
            // arrange
            var disaster = new Disaster() { Name = "name", IsActive = true };
            _mockService.Setup(m => m.AddDisaster(disaster)).Returns(disaster);
            DisasterService service = new DisasterService(_mockService.Object);

            // act
            var result = service.Create(disaster);

            // assert
            Assert.AreEqual(disaster.Name, result.Name);
            Assert.AreEqual(disaster.IsActive, result.IsActive);
            _mockService.Verify(m => m.AddDisaster(disaster));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateDisaster_DisasterNull()
        {
            var moqDataService = new Mock<IDataService>();
            DisasterService service = new DisasterService(moqDataService.Object);

            service.Create(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateDisaster_DisasterNameNull()
        {
            DisasterService service = new DisasterService(_mockService.Object);

            service.Create(new Disaster() { IsActive = true, Name = "" });
        }

        [TestMethod]
        public void GetList_ReturnsAllValid()
        {
            // arrange
            initializeDisasterCollection(_activeDisaster, _inActiveDisaster);

            var underTest = new DisasterService(_mockService.Object);

            // act
            var result = underTest.GetList();

            // assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetActiveList_ReturnsAllActiveValid()
        {
            // arrange
            initializeDisasterCollection(_activeDisaster, _inActiveDisaster);
            var underTest = new DisasterService(_mockService.Object);

            // act
            var result = underTest.GetActiveList();

            // assert
            Assert.AreEqual(1, result.Count());
        }

        //with empty collections, return an empty list.
        [TestMethod]
        public void WhenNoDisastersReturnAnEmptyList()
        {
            var underTest = new DisasterService(_mockService.Object);

            var result = underTest.GetList();
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void GetByID_Valid()
        {
            // arrange
            initializeDisasterCollection(_activeDisaster);
            var underTest = new DisasterService(_mockService.Object);

            // act
            var result = underTest.Get(_activeDisaster.Id);

            // assert
            Assert.AreEqual(_activeDisaster.Id, result.Id);
        }

        [TestMethod]
        public void GetByID_NotFound()
        {
            // arrange
           // initializeDisasterCollection(testDisaster1);
            var underTest = new DisasterService(_mockService.Object);

            // act
            var result = underTest.Get(_activeDisaster.Id);

            // assert
            Assert.AreEqual(null, result);
        }


        private void initializeDisasterCollection(params Disaster[] disasters)
        {
            _mockService.Setup(ds => ds.Disasters).Returns(disasters.AsQueryable());
        }
    }
}
