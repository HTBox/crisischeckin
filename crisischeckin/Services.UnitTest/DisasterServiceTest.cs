using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Models;
using Moq;
using Services.Interfaces;

namespace Services.UnitTest
{
    [TestFixture]
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

        [SetUp]
        public void CreateDependencies()
        {
            _mockDataService = new Mock<IDataService>();
            _mockDataService.Setup(s => s.Clusters).Returns(new List<Cluster>
            {
                new Cluster {Id = 1, Name="Sample" }
            }.AsQueryable());

            _disasterService = new DisasterService(_mockDataService.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullDataService()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new DisasterService(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AssignToVolunteer_BeginDateGreaterThanEndDate()
        {
            var startDate = new DateTime(2013, 6, 13);
            var endDate = startDate.Subtract(TimeSpan.FromDays(1));
            _disasterService.AssignToVolunteer(0, 0, startDate, endDate, 1, 1);
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void AssignToVolunteer_StartDateInThePast()
        {
            var startDate = DateTime.Today.AddDays(-10);
            var endDate = startDate.AddDays(1);
            _disasterService.AssignToVolunteer(0, 0, startDate, endDate, 1, 1);
        }

        [Test]
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

            _disasterService.AssignToVolunteer(0, 0, new DateTime(2013, 6, 11), new DateTime(2013, 6, 20), 1, 1);
        }

        [Test]
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

            _disasterService.AssignToVolunteer(0, 0, new DateTime(2013, 6, 5), new DateTime(2013, 6, 12), 1, 1);
        }

        [Test]
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

            _disasterService.AssignToVolunteer(0, 0, new DateTime(2013, 5, 6), new DateTime(2013, 5, 9), 1, 1);
        }

        [Test]
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

            _disasterService.AssignToVolunteer(0, 0, new DateTime(2013, 5, 7), new DateTime(2013, 5, 7), 1, 1);
        }

        [Test]
        public void AssignToVolunteer_StartAndEndDateBeforeExistingCommitment()
        {
            Commitment createdCommitment = null;
            _mockDataService.Setup(s => s.AddCommitment(It.IsAny<Commitment>()))
                .Callback<Commitment>(commitment => createdCommitment = commitment);

            var commitmentDate = DateTime.Today.AddDays(1);

            var commitments = new List<Commitment>
            {
                new Commitment
                {
                    StartDate = commitmentDate,
                    EndDate = commitmentDate,
                    DisasterId = 2
                }
            };
            var disasters = new List<Disaster> { new Disaster { Id = 2, IsActive = true } };

            _mockDataService.Setup(s => s.Commitments).Returns(commitments.AsQueryable());
            _mockDataService.Setup(s => s.Disasters).Returns(disasters.AsQueryable());

            var startDate = commitmentDate.AddDays(-1);
            var endDate = commitmentDate.AddDays(-1);
            _disasterService.AssignToVolunteer(0, 0, startDate, endDate, 1, 1);

            Assert.IsNotNull(createdCommitment);
            Assert.AreEqual(startDate, createdCommitment.StartDate);
            Assert.AreEqual(endDate, createdCommitment.EndDate);
        }

        [Test]
        public void AssignToVolunteer_StartAndEndDateAfterExistingCommitment()
        {
            Commitment createdCommitment = null;
            _mockDataService.Setup(s => s.AddCommitment(It.IsAny<Commitment>()))
                .Callback<Commitment>(commitment => createdCommitment = commitment);

            var commitmentDate = DateTime.Today;

            var commitments = new List<Commitment>
            {
                new Commitment
                {
                    StartDate = commitmentDate,
                    EndDate = commitmentDate,
                    DisasterId = 2
                }
            };
            var disasters = new List<Disaster> { new Disaster { Id = 2, IsActive = true } };

            _mockDataService.Setup(s => s.Commitments).Returns(commitments.AsQueryable());
            _mockDataService.Setup(s => s.Disasters).Returns(disasters.AsQueryable());

            var startDate = commitmentDate.AddDays(1);
            var endDate = commitmentDate.AddDays(1);
            _disasterService.AssignToVolunteer(0, 0, startDate, endDate, 1, 1);

            Assert.IsNotNull(createdCommitment);
            Assert.AreEqual(startDate, createdCommitment.StartDate);
            Assert.AreEqual(endDate, createdCommitment.EndDate);
        }

        [Test]
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

            _disasterService.AssignToVolunteer(0, 0, new DateTime(2013, 5, 10), new DateTime(2013, 5, 10), 1, 1);
        }

        [Test]
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

            _disasterService.AssignToVolunteer(0, 0, new DateTime(2013, 5, 6), new DateTime(2013, 5, 6), 1, 1);
        }

        [Test]
        public void AssignToVolunteer_Valid()
        {
            Commitment createdCommitment = null;
            _mockDataService.Setup(s => s.AddCommitment(It.IsAny<Commitment>()))
                .Callback<Commitment>(commitment => createdCommitment = commitment);

            const int personId = 5;
            const int disasterId = 10;
            var startDate = DateTime.Today;
            var endDate = startDate.AddDays(1);
            _disasterService.AssignToVolunteer(disasterId, personId, startDate, endDate, 1, 1);

            Assert.IsNotNull(createdCommitment);
            Assert.AreEqual(createdCommitment.PersonId, personId);
            Assert.AreEqual(createdCommitment.DisasterId, disasterId);
            Assert.AreEqual(createdCommitment.StartDate, startDate);
            Assert.AreEqual(createdCommitment.EndDate, endDate);
        }

        [Test]
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
            var startDate = DateTime.Today;
            var endDate = startDate.AddDays(1);

            Commitment createdCommitment = null;
            _mockDataService.Setup(s => s.AddCommitment(It.IsAny<Commitment>()))
                .Callback<Commitment>(commitment => createdCommitment = commitment);

            _disasterService.AssignToVolunteer(disasterId, personId, startDate, endDate, 1, 1);
            Assert.IsNotNull(createdCommitment);
            Assert.AreEqual(createdCommitment.PersonId, personId);
            Assert.AreEqual(createdCommitment.DisasterId, disasterId);
            Assert.AreEqual(createdCommitment.StartDate, startDate);
            Assert.AreEqual(createdCommitment.EndDate, endDate);
        }

        [Test]
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

        [Test]
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

        [Test]
        [ExpectedException(typeof(Services.Exceptions.DisasterAlreadyExistsException))]
        public void CreateDisaster_SameName_ThrowDisasterAlreadyExistsException()
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

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateDisaster_DisasterNull()
        {
            _disasterService.Create(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateDisaster_DisasterNameNull()
        {
            _disasterService.Create(new Disaster { IsActive = true, Name = "" });
        }

        [Test]
        public void GetList_ReturnsAllValid()
        {
            // arrange
            InitializeDisasterCollection(_activeDisaster, _inactiveDisaster);

            // act
            var result = _disasterService.GetList();

            // assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
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
        [Test]
        public void WhenNoDisastersReturnAnEmptyList()
        {
            var result = _disasterService.GetList();
            Assert.IsFalse(result.Any());
        }

        [Test]
        public void GetByID_Valid()
        {
            // arrange
            InitializeDisasterCollection(_activeDisaster);

            // act
            var result = _disasterService.Get(_activeDisaster.Id);

            // assert
            Assert.AreEqual(_activeDisaster.Id, result.Id);
        }

        [Test]
        public void GetByID_NotFound()
        {
            // arrange

            // act
            var result = _disasterService.Get(_activeDisaster.Id);

            // assert
            Assert.AreEqual(null, result);
        }

        [Test]
        public void UpdateDisaster_Valid()
        {
            // arrange
            var dataList = new List<Disaster>{
                new Disaster {Id= 1, Name = "name", IsActive = true },
                new Disaster {Id = 2, Name = "name2", IsActive = true }
            }.AsQueryable();

            _mockDataService.Setup(m => m.Disasters).Returns(dataList);

            Disaster updatedDisasterResult = null;
            _mockDataService.Setup(m => m.UpdateDisaster(It.IsAny<Disaster>())).Callback<Disaster>(d => updatedDisasterResult = d);

            var updatedDisaster = new Disaster { Id = 1, Name = "name", IsActive = false };

            // act
            _disasterService.Update(updatedDisaster);

            // assert
            Assert.AreEqual(updatedDisaster.Name, updatedDisasterResult.Name);
            Assert.AreEqual(updatedDisaster.IsActive, updatedDisasterResult.IsActive);
            _mockDataService.Verify(m => m.UpdateDisaster(updatedDisaster));
        }

        [Test]
        [ExpectedException(typeof(Services.Exceptions.DisasterAlreadyExistsException))]
        public void UpdateDisaster_SameName_ThrowsDisasterAlreadyExistsException()
        {
            // arrange
            var dataList = new List<Disaster>{
                new Disaster {Id= 1, Name = "name", IsActive = true },
                new Disaster {Id = 2, Name = "name2", IsActive = true }
            }.AsQueryable();

            _mockDataService.Setup(m => m.Disasters).Returns(dataList);

            Disaster updatedDisasterResult = null;
            _mockDataService.Setup(m => m.UpdateDisaster(It.IsAny<Disaster>())).Callback<Disaster>(d => updatedDisasterResult = d);

            var updatedDisaster = new Disaster { Id = 2, Name = "name", IsActive = true };

            // act
            _disasterService.Update(updatedDisaster);

            // assert
            // Should throw a DisasterAlreadyExistsException
        }

        [Test]
        [ExpectedException(typeof(Services.Exceptions.DisasterNotFoundException))]
        public void UpdateDisaster_InvalidId_ThrowsDisasterNotFoundException()
        {
            // arrange
            var dataList = new List<Disaster>{
                new Disaster {Id= 1, Name = "name", IsActive = true },
                new Disaster {Id = 2, Name = "name2", IsActive = true }
            }.AsQueryable();

            _mockDataService.Setup(m => m.Disasters).Returns(dataList);

            Disaster updatedDisasterResult = null;
            _mockDataService.Setup(m => m.UpdateDisaster(It.IsAny<Disaster>())).Callback<Disaster>(d => updatedDisasterResult = d);

            var updatedDisaster = new Disaster { Id = 5, Name = "name", IsActive = true };

            // act
            _disasterService.Update(updatedDisaster);

            // assert
            // Should throw a DisasterNotFoundException
        }
        
        private void InitializeDisasterCollection(params Disaster[] disasters)
        {
            _mockDataService.Setup(ds => ds.Disasters).Returns(disasters.AsQueryable());
        }
    }
}
