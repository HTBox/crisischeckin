using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;
using Services.Interfaces;

namespace Services.UnitTest.ClusterCoordinatorService
{
    [TestClass]
    public class GetClusterCoordinatorTests
    {
        Services.ClusterCoordinatorService _clusterCoordinatorService;
        Mock<IDataService> _dataService;
        int _coordinatorId;
        int _notCoordinatorId;
        private Person _person1;
        private Cluster _cluster1;
        private Disaster _disaster1;
        private Person _person2;
        private Cluster _cluster2;
        private Disaster _disaster2;
        private Person _person3;

        [TestInitialize]
        public void Init()
        {
            _coordinatorId = 42;
            _notCoordinatorId = 10;
            _dataService = new Mock<IDataService>();

            _disaster1 = new Disaster { Id = 1, IsActive = true, Name = "Sharknado" };
            _disaster2 = new Disaster { Id = 2, IsActive = true, Name = "Ice Age" };

            _cluster1 = new Cluster { Id = 2, Name = "Red Zone" };
            _cluster2 = new Cluster { Id = 3, Name = "Other Cluster" };

            _person1 = new Person { Id = 3, FirstName = "John", LastName = "Doe" };
            _person2 = new Person { Id = 4, FirstName = "Richard", LastName = "Roe" };
            _person3 = new Person { Id = 5, FirstName = "Little", LastName = "Toe" };

            _dataService.Setup(x => x.Disasters).Returns(new EnumerableQuery<Disaster>(new[]
            {
                _disaster1,
                _disaster2
            }));
            _dataService.Setup(x => x.Clusters).Returns(new EnumerableQuery<Cluster>(new[]
            {
                _cluster1,
                _cluster2
            }));
            _dataService.Setup(x => x.Persons).Returns(new EnumerableQuery<Person>(new[]
            {
                _person1,
                _person2,
                _person3
            }));
            _dataService.Setup(x => x.Commitments).Returns(new EnumerableQuery<Commitment>(new[]
            {
                new Commitment { DisasterId = _disaster1.Id, PersonId = _person1.Id },
                new Commitment { DisasterId = _disaster1.Id, PersonId = _person2.Id },
                new Commitment { DisasterId = _disaster2.Id, PersonId = _person3.Id }
            }));
            _clusterCoordinatorService = new Services.ClusterCoordinatorService(_dataService.Object);
        }

        [TestMethod]
        public void GetClusterCoordinator_returns_the_expected_coordinator()
        {
            var coordinators = new EnumerableQuery<ClusterCoordinator>(new[]
                                                                       {
                                                                           new ClusterCoordinator {Id = _notCoordinatorId},
                                                                           new ClusterCoordinator {Id = _coordinatorId},
                                                                       });
            _dataService.Setup(x => x.ClusterCoordinators).Returns(coordinators);

            var clusterCoordinator = _clusterCoordinatorService.GetCoordinator(_coordinatorId);

            Assert.AreEqual(_coordinatorId, clusterCoordinator.Id);
        }

        [TestMethod]
        public void ClusterCoordinatorGetAllCoordinatorsForDisplayReturnsExpectedData()
        {
            // Arrange
            var coordinators = new EnumerableQuery<ClusterCoordinator>(new[]
                                                                       {
                                                                           new ClusterCoordinator
                                                                           {
                                                                               Id = _coordinatorId,
                                                                               PersonId = _person1.Id,
                                                                               ClusterId = _cluster1.Id,
                                                                               DisasterId = _disaster1.Id,
                                                                               Person = _person1,
                                                                               Cluster = _cluster1,
                                                                               Disaster = _disaster1
                                                                           },
                                                                           new ClusterCoordinator
                                                                           {
                                                                               Id = _notCoordinatorId,
                                                                               PersonId = _person2.Id,
                                                                               ClusterId = _cluster2.Id,
                                                                               DisasterId = _disaster1.Id,
                                                                               Person = _person2,
                                                                               Cluster = _cluster2,
                                                                               Disaster = _disaster1
                                                                           }
                                                                       });
            _dataService.Setup(x => x.ClusterCoordinators).Returns(coordinators);
            IList<Person> allPersonsForDisplay;

            // Act
            var clusterCoordinators = _clusterCoordinatorService.GetAllCoordinatorsForDisplay(_disaster1.Id,
                out allPersonsForDisplay).ToList();

            // Assert
            Assert.IsNotNull(clusterCoordinators);
            Assert.IsNotNull(allPersonsForDisplay);
            Assert.AreEqual(2, clusterCoordinators.Count());
            Assert.AreEqual(2, allPersonsForDisplay.Count());
            const string errorMsg = "Could not find the coordinator with the ID of ";
            var firstCoordinator = clusterCoordinators.FirstOrDefault(cc => cc.Id == _coordinatorId);
            Assert.IsNotNull(firstCoordinator, errorMsg + _coordinatorId);
            var secondCoordinator = clusterCoordinators.FirstOrDefault(cc => cc.Id == _notCoordinatorId);
            Assert.IsNotNull(secondCoordinator, errorMsg + _notCoordinatorId);
            Assert.AreEqual(_person1.Id, firstCoordinator.PersonId);
            Assert.AreEqual(_person2.Id, secondCoordinator.PersonId);
            const string personErrorMsg = "Could not find person with the ID of ";
            var firstPerson = allPersonsForDisplay.FirstOrDefault(x => x.Id == _person1.Id);
            Assert.IsNotNull(firstPerson, personErrorMsg + _person1.Id);
            var thirdPerson = allPersonsForDisplay.FirstOrDefault(x => x.Id == _person3.Id);
            Assert.IsNull(thirdPerson, "Third person has not volunteerd for disaster " + _disaster1.Id);

        }

        [TestMethod]
        public void GetAllCoordinatorsForClusterTest()
        {
            // Arrange
            var coordinators = new EnumerableQuery<ClusterCoordinator>(new[]
                                                                       {
                                                                           new ClusterCoordinator
                                                                           {
                                                                               Id = _coordinatorId,
                                                                               PersonId = _person1.Id,
                                                                               ClusterId = _cluster1.Id,
                                                                               DisasterId = _disaster1.Id,
                                                                               Person = _person1,
                                                                               Cluster = _cluster1,
                                                                               Disaster = _disaster1
                                                                           },
                                                                           new ClusterCoordinator
                                                                           {
                                                                               Id = _notCoordinatorId,
                                                                               PersonId = _person2.Id,
                                                                               ClusterId = _cluster2.Id,
                                                                               DisasterId = _disaster1.Id,
                                                                               Person = _person2,
                                                                               Cluster = _cluster2,
                                                                               Disaster = _disaster1
                                                                           }
                                                                       });
            _dataService.Setup(x => x.ClusterCoordinators).Returns(coordinators);

            // Act
            var clusterCoordinators = _clusterCoordinatorService.GetAllCoordinatorsForCluster(2).ToList();

            // Assert
            Assert.IsNotNull(clusterCoordinators);
            Assert.AreEqual(1, clusterCoordinators.Count());
            const string ErrorMsg = "Could not find the coordinator with the ID of ";
            var firstCoordinator = clusterCoordinators.FirstOrDefault(cc => cc.Id == _coordinatorId);
            Assert.IsNotNull(firstCoordinator, ErrorMsg + _coordinatorId);
            Assert.AreEqual(_person1.Id, firstCoordinator.PersonId);
        }

        [TestMethod]
        public void GetCoordinatorFullyLoadedReturnsExpectedData()
        {
            var coordinators = new EnumerableQuery<ClusterCoordinator>(new[]
                                                                       {
                                                                           new ClusterCoordinator
                                                                           {
                                                                               Id = _coordinatorId,
                                                                               PersonId = _person1.Id,
                                                                               ClusterId = _cluster1.Id,
                                                                               DisasterId = _disaster1.Id,
                                                                               Person = _person1,
                                                                               Cluster = _cluster1,
                                                                               Disaster = _disaster1
                                                                           },
                                                                           new ClusterCoordinator
                                                                           {
                                                                               Id = _notCoordinatorId,
                                                                               PersonId = _person2.Id,
                                                                               ClusterId = _cluster2.Id,
                                                                               DisasterId = _disaster1.Id,
                                                                               Person = _person2,
                                                                               Cluster = _cluster2,
                                                                               Disaster = _disaster1
                                                                           }
                                                                       });
            _dataService.Setup(x => x.ClusterCoordinators).Returns(coordinators);

            var coordinator = _clusterCoordinatorService.GetCoordinatorFullyLoaded(_notCoordinatorId);

            Assert.IsNotNull(coordinator);
            Assert.AreEqual(_notCoordinatorId, coordinator.Id);
            Assert.AreEqual(_person2.Id, coordinator.PersonId);
            Assert.AreEqual(_cluster2.Id, coordinator.ClusterId);
            Assert.AreEqual(_disaster1.Id, coordinator.DisasterId);
            Assert.IsNotNull(coordinator.Person);
            Assert.IsNotNull(coordinator.Disaster);
            Assert.IsNotNull(coordinator.Cluster);
        }

        [TestMethod]
        public void GetCoordinatorForUnassignReturnsExpectedData()
        {
            var coordinators = new EnumerableQuery<ClusterCoordinator>(new[]
                                                                       {
                                                                           new ClusterCoordinator
                                                                           {
                                                                               Id = _coordinatorId,
                                                                               PersonId = _person1.Id,
                                                                               ClusterId = _cluster1.Id,
                                                                               DisasterId = _disaster1.Id,
                                                                               Person = _person1,
                                                                               Cluster = _cluster1,
                                                                               Disaster = _disaster1
                                                                           },
                                                                           new ClusterCoordinator
                                                                           {
                                                                               Id = _notCoordinatorId,
                                                                               PersonId = _person2.Id,
                                                                               ClusterId = _cluster2.Id,
                                                                               DisasterId = _disaster1.Id,
                                                                               Person = _person2,
                                                                               Cluster = _cluster2,
                                                                               Disaster = _disaster1
                                                                           }
                                                                       });
            _dataService.Setup(x => x.ClusterCoordinators).Returns(coordinators);

            var coordinator = _clusterCoordinatorService.GetCoordinatorFullyLoaded(_coordinatorId);

            Assert.IsNotNull(coordinator);
            Assert.AreEqual(_coordinatorId, coordinator.Id);
            Assert.AreEqual(_person1.Id, coordinator.PersonId);
            Assert.AreEqual(_cluster1.Id, coordinator.ClusterId);
            Assert.AreEqual(_disaster1.Id, coordinator.DisasterId);
            Assert.IsNotNull(coordinator.Person);
            Assert.IsNotNull(coordinator.Disaster);
            Assert.IsNotNull(coordinator.Cluster);
            Assert.AreEqual(_person1.FullName, coordinator.Person.FullName);
            Assert.AreEqual(_cluster1.Name, coordinator.Cluster.Name);
            Assert.AreEqual(_disaster1.Name, coordinator.Disaster.Name);
        }
    }
}