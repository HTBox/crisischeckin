using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;
using Services.Interfaces;

namespace Services.UnitTest
{
    [TestClass]
    public class DisasterServiceTest
    {

        private readonly Disaster activeDisaster = new Disaster
        {
            Id = 1,
            Name = "Test Disaster",
            IsActive = true
        };

        private readonly Disaster inActiveDisaster = new Disaster
        {
            Id = 2,
            Name = "Test Disaster 2",
            IsActive = false
        };

        private Mock<IDataService> mockService;

        [TestInitialize]
        public void CreateDependencies()
        {
            mockService = new Mock<IDataService>();
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
        public void AssignToVolunteer_Valid()
        {
            var moqDataService = new Mock<IDataService>();
            moqDataService.Setup(s => s.AddCommitment(It.IsAny<Commitment>())).Returns(new Commitment()
            {
                Id = 1,
                PersonId = 5,
                DisasterId = 10,
                StartDate = new DateTime(2014, 01, 01),
                EndDate = new DateTime(2014, 01, 01)
            });
            DisasterService service = new DisasterService(moqDataService.Object);

            var actual = service.AssignToVolunteer(new Disaster() { Id = 10, Name = "A disaster" },
                new Person() { Id = 5, Email = "bob.jones@email.com" },
                new DateTime(2013, 01, 01), new DateTime(2013, 02, 01));

            Assert.AreEqual(1, actual.Id);
            Assert.AreEqual(5, actual.PersonId);
            Assert.AreEqual(10, actual.DisasterId);
            Assert.AreEqual("1/1/2014", actual.StartDate.ToShortDateString());
        }

        [TestMethod]
        public void CreateDisaster_Valid()
        {
            // arrange
            var disaster = new Disaster() { Name = "name", IsActive = true };
            mockService.Setup(m => m.AddDisaster(disaster)).Returns(disaster);
            DisasterService service = new DisasterService(mockService.Object);

            // act
            var result = service.Create(disaster);

            // assert
            Assert.AreEqual(disaster.Name, result.Name);
            Assert.AreEqual(disaster.IsActive, result.IsActive);
            mockService.Verify(m => m.AddDisaster(disaster));
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
            DisasterService service = new DisasterService(mockService.Object);

            service.Create(new Disaster() { IsActive = true, Name = "" });
        }

        [TestMethod]
        public void GetList_ReturnsAllValid()
        {
            // arrange
            initializeDisasterCollection(activeDisaster, inActiveDisaster);

            var underTest = new DisasterService(mockService.Object);

            // act
            var result = underTest.GetList();

            // assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetActiveList_ReturnsAllActiveValid()
        {
            // arrange
            initializeDisasterCollection(activeDisaster, inActiveDisaster);
            var underTest = new DisasterService(mockService.Object);

            // act
            var result = underTest.GetActiveList();

            // assert
            Assert.AreEqual(1, result.Count());
        }

        //with empty collections, return an empty list.
        [TestMethod]
        public void WhenNoDisastersReturnAnEmptyList()
        {
            var underTest = new DisasterService(mockService.Object);

            var result = underTest.GetList();
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void GetByID_Valid()
        {
            // arrange
            initializeDisasterCollection(activeDisaster);
            var underTest = new DisasterService(mockService.Object);

            // act
            var result = underTest.Get(activeDisaster.Id);

            // assert
            Assert.AreEqual(activeDisaster.Id, result.Id);
        }

        [TestMethod]
        public void GetByID_NotFound()
        {
            // arrange
           // initializeDisasterCollection(testDisaster1);
            var underTest = new DisasterService(mockService.Object);

            // act
            var result = underTest.Get(activeDisaster.Id);

            // assert
            Assert.AreEqual(null, result);
        }


        private void initializeDisasterCollection(params Disaster[] disasters)
        {
            mockService.Setup(ds => ds.Disasters).Returns(disasters.AsQueryable());
        }
    }
}
