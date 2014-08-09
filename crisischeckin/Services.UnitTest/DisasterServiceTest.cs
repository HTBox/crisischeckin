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

        private readonly Disaster _activeDisaster = new Disaster
        {
            Id = 1,
            Name = "Test Disaster",
            IsActive = true
        };

        private readonly Disaster _inactiveDisaster = new Disaster
        {
            Id = 2,
            Name = "Test Disaster 2",
            IsActive = false
        };

        private Mock<IDataService> _mockDataService;
        private DisasterService _disasterService;

        [TestInitialize]
        public void CreateDependencies()
        {
            _mockDataService = new Mock<IDataService>();
            _disasterService = new DisasterService(_mockDataService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullDataService()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new DisasterService(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AssignToVolunteer_BeginDateGreaterThanEndDate()
        {
            var startDate = new DateTime(2013, 6, 13);
            var endDate = startDate.Subtract(TimeSpan.FromDays(1));
            _disasterService.AssignToVolunteer(0, 0, startDate, endDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AssignToVolunteer_StartDateIsWithinExistingCommitment()
        {
            var commitments = new List<Commitment>
            {
                new Commitment
                {
                    StartDate = new DateTime(2013, 6, 10),
                    EndDate = new DateTime(2013, 6, 15),
                    DisasterId = 2
                }
            };
            var disasters = new List<Disaster> { new Disaster { Id = 2, IsActive = true } };

            _mockDataService.Setup(s => s.Commitments).Returns(commitments.AsQueryable());
            _mockDataService.Setup(s => s.Disasters).Returns(disasters.AsQueryable());

            _disasterService.AssignToVolunteer(0, 0, new DateTime(2013, 6, 11), new DateTime(2013, 6, 20));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AssignToVolunteer_EndDateIsWithinExistingCommitment()
        {
            var commitments = new List<Commitment>
            {
                new Commitment
                {
                    StartDate = new DateTime(2013, 6, 10),
                    EndDate = new DateTime(2013, 6, 15),
                    DisasterId = 2
                }
            };
            var disasters = new List<Disaster> { new Disaster { Id = 2, IsActive = true } };

            _mockDataService.Setup(s => s.Commitments).Returns(commitments.AsQueryable());
            _mockDataService.Setup(s => s.Disasters).Returns(disasters.AsQueryable());

            _disasterService.AssignToVolunteer(0, 0, new DateTime(2013, 6, 5), new DateTime(2013, 6, 12));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AssignToVolunteer_StartAndEndDateSpanExistingCommitment()
        {
            var commitments = new List<Commitment>
            {
                new Commitment
                {
                    StartDate = new DateTime(2013, 5, 7),
                    EndDate = new DateTime(2013, 5, 8),
                    DisasterId = 2
                }
            };
            var disasters = new List<Disaster> { new Disaster { Id = 2, IsActive = true } };

            _mockDataService.Setup(s => s.Commitments).Returns(commitments.AsQueryable());
            _mockDataService.Setup(s => s.Disasters).Returns(disasters.AsQueryable());

            _disasterService.AssignToVolunteer(0, 0, new DateTime(2013, 5, 6), new DateTime(2013, 5, 9));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AssignToVolunteer_StartAndEndDateAreWithinExistingCommitment()
        {
            var commitments = new List<Commitment>
            {
                new Commitment
                {
                    StartDate = new DateTime(2013, 5, 6),
                    EndDate = new DateTime(2013, 5, 8),
                    DisasterId = 2
                }
            };
            var disasters = new List<Disaster> { new Disaster { Id = 2, IsActive = true } };

            _mockDataService.Setup(s => s.Commitments).Returns(commitments.AsQueryable());
            _mockDataService.Setup(s => s.Disasters).Returns(disasters.AsQueryable());

            _disasterService.AssignToVolunteer(0, 0, new DateTime(2013, 5, 7), new DateTime(2013, 5, 7));
        }

        [TestMethod]
        public void AssignToVolunteer_StartAndEndDateBeforeExistingCommitment()
        {
            Commitment createdCommitment = null;
            _mockDataService.Setup(s => s.AddCommitment(It.IsAny<Commitment>()))
                .Callback<Commitment>(commitment => createdCommitment = commitment);

            var commitments = new List<Commitment>
            {
                new Commitment
                {
                    StartDate = new DateTime(2013, 5, 8),
                    EndDate = new DateTime(2013, 5, 8),
                    DisasterId = 2
                }
            };
            var disasters = new List<Disaster> { new Disaster { Id = 2, IsActive = true } };

            _mockDataService.Setup(s => s.Commitments).Returns(commitments.AsQueryable());
            _mockDataService.Setup(s => s.Disasters).Returns(disasters.AsQueryable());

            var startDate = new DateTime(2013, 5, 7);
            var endDate = new DateTime(2013, 5, 7);
            _disasterService.AssignToVolunteer(0, 0, startDate, endDate);

            Assert.IsNotNull(createdCommitment);
            Assert.AreEqual(startDate, createdCommitment.StartDate);
            Assert.AreEqual(endDate, createdCommitment.EndDate);
        }

        [TestMethod]
        public void AssignToVolunteer_StartAndEndDateAfterExistingCommitment()
        {
            Commitment createdCommitment = null;
            _mockDataService.Setup(s => s.AddCommitment(It.IsAny<Commitment>()))
                .Callback<Commitment>(commitment => createdCommitment = commitment);

            var commitments = new List<Commitment>
            {
                new Commitment
                {
                    StartDate = new DateTime(2013, 5, 6),
                    EndDate = new DateTime(2013, 5, 6),
                    DisasterId = 2
                }
            };
            var disasters = new List<Disaster> { new Disaster { Id = 2, IsActive = true } };

            _mockDataService.Setup(s => s.Commitments).Returns(commitments.AsQueryable());
            _mockDataService.Setup(s => s.Disasters).Returns(disasters.AsQueryable());

            var startDate = new DateTime(2013, 5, 7);
            var endDate = new DateTime(2013, 5, 7);
            _disasterService.AssignToVolunteer(0, 0, startDate, endDate);

            Assert.IsNotNull(createdCommitment);
            Assert.AreEqual(startDate, createdCommitment.StartDate);
            Assert.AreEqual(endDate, createdCommitment.EndDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AssignToVolunteer_StartDateSameAsExistingCommitmentEndDate()
        {
            Commitment createdCommitment = null;
            _mockDataService.Setup(s => s.AddCommitment(It.IsAny<Commitment>()))
                .Callback<Commitment>(commitment => createdCommitment = commitment);

            var commitments = new List<Commitment>
            {
                new Commitment
                {
                    StartDate = new DateTime(2013, 5, 6),
                    EndDate = new DateTime(2013, 5, 10),
                    DisasterId = 2
                }
            };
            var disasters = new List<Disaster> { new Disaster { Id = 2, IsActive = true } };

            _mockDataService.Setup(s => s.Commitments).Returns(commitments.AsQueryable());
            _mockDataService.Setup(s => s.Disasters).Returns(disasters.AsQueryable());

            _disasterService.AssignToVolunteer(0, 0, new DateTime(2013, 5, 10), new DateTime(2013, 5, 10));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AssignToVolunteer_EndDateSameAsExistingCommitmentStartDate()
        {
            Commitment createdCommitment = null;
            _mockDataService.Setup(s => s.AddCommitment(It.IsAny<Commitment>()))
                .Callback<Commitment>(commitment => createdCommitment = commitment);

            var commitments = new List<Commitment>
            {
                new Commitment
                {
                    StartDate = new DateTime(2013, 5, 6),
                    EndDate = new DateTime(2013, 5, 10),
                    DisasterId = 2
                }
            };
            var disasters = new List<Disaster> { new Disaster { Id = 2, IsActive = true } };

            _mockDataService.Setup(s => s.Commitments).Returns(commitments.AsQueryable());
            _mockDataService.Setup(s => s.Disasters).Returns(disasters.AsQueryable());

            _disasterService.AssignToVolunteer(0, 0, new DateTime(2013, 5, 6), new DateTime(2013, 5, 6));
        }

        [TestMethod]
        public void AssignToVolunteer_Valid()
        {
            Commitment createdCommitment = null;
            _mockDataService.Setup(s => s.AddCommitment(It.IsAny<Commitment>()))
                .Callback<Commitment>(commitment => createdCommitment = commitment);

            const int personId = 5;
            const int disasterId = 10;
            var startDate = new DateTime(2013, 01, 01);
            var endDate = new DateTime(2013, 02, 01);
            _disasterService.AssignToVolunteer(disasterId, personId, startDate, endDate);

            Assert.IsNotNull(createdCommitment);
            Assert.AreEqual(createdCommitment.PersonId, personId);
            Assert.AreEqual(createdCommitment.DisasterId, disasterId);
            Assert.AreEqual(createdCommitment.StartDate, startDate);
            Assert.AreEqual(createdCommitment.EndDate, endDate);
        }

        [TestMethod]
        public void AssignToVolunteer_ValidForMoreThanOneVolunteerPerDisaster()
        {
            var commitments = new List<Commitment>
            {
                new Commitment
                {
                    PersonId = 0,
                    StartDate = new DateTime(2013, 1, 1),
                    EndDate = new DateTime(2013, 2, 1),
                    DisasterId = 2
                }
            };
            var disasters = new List<Disaster> { new Disaster { Id = 2, IsActive = true } };

            _mockDataService.Setup(s => s.Commitments).Returns(commitments.AsQueryable());
            _mockDataService.Setup(s => s.Disasters).Returns(disasters.AsQueryable());

            const int personId = 5;
            const int disasterId = 2;
            var startDate = new DateTime(2013, 01, 02);
            var endDate = new DateTime(2013, 01, 03);

            Commitment createdCommitment = null;
            _mockDataService.Setup(s => s.AddCommitment(It.IsAny<Commitment>()))
                .Callback<Commitment>(commitment => createdCommitment = commitment);

            _disasterService.AssignToVolunteer(disasterId, personId, startDate, endDate);
            Assert.IsNotNull(createdCommitment);
            Assert.AreEqual(createdCommitment.PersonId, personId);
            Assert.AreEqual(createdCommitment.DisasterId, disasterId);
            Assert.AreEqual(createdCommitment.StartDate, startDate);
            Assert.AreEqual(createdCommitment.EndDate, endDate);
        }

        [TestMethod]
        public void RemoveCommitmentById_Valid()
        {
            // arrange
            var commitmentId = int.MinValue;
            var removeCommitmentByIdMethodCalled = false;
            _mockDataService.Setup(m => m.RemoveCommitmentById(It.IsAny<int>())).Callback(() => removeCommitmentByIdMethodCalled = true);

            // act
            _disasterService.RemoveCommitmentById(commitmentId);

            // assert
            Assert.IsTrue(removeCommitmentByIdMethodCalled);
        }

        [TestMethod]
        public void CreateDisaster_Valid()
        {
            // arrange
            var disaster = new Disaster { Name = "name", IsActive = true };
            Disaster createdDisaster = null;
            _mockDataService.Setup(m => m.AddDisaster(disaster)).Callback<Disaster>(d => createdDisaster = d);

            // act
            _disasterService.Create(disaster);

            // assert
            Assert.AreEqual(disaster.Name, createdDisaster.Name);
            Assert.AreEqual(disaster.IsActive, createdDisaster.IsActive);
            _mockDataService.Verify(m => m.AddDisaster(disaster));
        }

        [TestMethod]
        [ExpectedException(typeof(Services.Exceptions.DisasterAlreadyExistsException))]
        public void UpdateDisaster_SameName_ThrowDisasterAlreadyExistsException()
        {
            // arrange
            var dataList = new List<Disaster>{
                new Disaster {Id= 1, Name = "name", IsActive = true },
                new Disaster {Id = 2, Name = "name2", IsActive = true }
            }.AsQueryable();

            _mockDataService.Setup(m => m.Disasters).Returns(dataList);

            var duplicateDisaster = new Disaster { Id = 2, Name = "name2", IsActive = true };

            //Action
            _disasterService.Create(duplicateDisaster);

            //Assert
            //Exception should be thrown
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateDisaster_DisasterNull()
        {
            _disasterService.Create(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateDisaster_DisasterNameNull()
        {
            _disasterService.Create(new Disaster { IsActive = true, Name = "" });
        }

        [TestMethod]
        public void GetList_ReturnsAllValid()
        {
            // arrange
            InitializeDisasterCollection(_activeDisaster, _inactiveDisaster);

            // act
            var result = _disasterService.GetList();

            // assert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetActiveList_ReturnsAllActiveValid()
        {
            // arrange
            InitializeDisasterCollection(_activeDisaster, _inactiveDisaster);

            // act
            var result = _disasterService.GetActiveList();

            // assert
            Assert.AreEqual(1, result.Count());
        }

        //with empty collections, return an empty list.
        [TestMethod]
        public void WhenNoDisastersReturnAnEmptyList()
        {
            var result = _disasterService.GetList();
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void GetByID_Valid()
        {
            // arrange
            InitializeDisasterCollection(_activeDisaster);

            // act
            var result = _disasterService.Get(_activeDisaster.Id);

            // assert
            Assert.AreEqual(_activeDisaster.Id, result.Id);
        }

        [TestMethod]
        public void GetByID_NotFound()
        {
            // arrange

            // act
            var result = _disasterService.Get(_activeDisaster.Id);

            // assert
            Assert.AreEqual(null, result);
        }


        private void InitializeDisasterCollection(params Disaster[] disasters)
        {
            _mockDataService.Setup(ds => ds.Disasters).Returns(disasters.AsQueryable());
        }
    }
}
