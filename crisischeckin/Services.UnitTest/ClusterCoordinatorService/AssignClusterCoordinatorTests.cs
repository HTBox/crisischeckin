using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Models;
using Moq;
using Services.Interfaces;

namespace Services.UnitTest.ClusterCoordinatorService
{
    [TestFixture]
    public class AssignClusterCoordinatorTests
    {
        Cluster _cluster;
        Services.ClusterCoordinatorService _clusterCoordinatorService;
        Mock<IDataService> _dataService;
        Disaster _disaster;
        Person _person;

        [SetUp]
        public void Init()
        {
            _disaster = new Disaster {Id = 0, IsActive = true, Name = "Sharknado"};
            _cluster = new Cluster {Id = 1, Name = "Red Zone"};
            _person = new Person {Id = 2, FirstName = "John", LastName = "Doe"};

            _dataService = new Mock<IDataService>();
            _dataService.Setup(x => x.Disasters).Returns(new EnumerableQuery<Disaster>(new[] {_disaster}));
            _dataService.Setup(x => x.Clusters).Returns(new EnumerableQuery<Cluster>(new[] {_cluster}));
            _dataService.Setup(x => x.Persons).Returns(new EnumerableQuery<Person>(new[] {_person}));

            _clusterCoordinatorService = new Services.ClusterCoordinatorService(_dataService.Object);
        }

        [Test]
        public void AssignClusterCoordinator_saves_a_new_ClusterCoordinatorRecord()
        {
            var clusterCoordinator = new ClusterCoordinator
            {
                Id = 1,
                DisasterId = _disaster.Id,
                ClusterId = _cluster.Id,
                PersonId = _person.Id,
                Person = _person,
                Cluster =  _cluster,
                Disaster = _disaster
            };
            var coordinatorList = new List<ClusterCoordinator>();
            _dataService.Setup(x => x.ClusterCoordinators).Returns(coordinatorList.AsQueryable());
            _dataService.Setup(x => x.AddClusterCoordinator(It.Is<ClusterCoordinator>(cc => cc.DisasterId == _disaster.Id &&
                                                                                            cc.ClusterId == _cluster.Id &&
                                                                                            cc.PersonId == _person.Id)))
                .Callback(() => coordinatorList.Add(clusterCoordinator));

            _clusterCoordinatorService.AssignClusterCoordinator(clusterCoordinator.DisasterId, clusterCoordinator.ClusterId, clusterCoordinator.PersonId);

            _dataService.Verify(x => x.AddClusterCoordinator(It.IsAny<ClusterCoordinator>()), Times.Once());
            _dataService.Verify(x => x.AddClusterCoordinator(It.Is<ClusterCoordinator>(cc => cc.DisasterId == clusterCoordinator.DisasterId &&
                                                                                             cc.ClusterId == clusterCoordinator.ClusterId &&
                                                                                             cc.PersonId == clusterCoordinator.PersonId)));
        }

        [Test]
        public void AssignClusterCoordinator_with_no_cluster_id_does_nothing()
        {
            var result = _clusterCoordinatorService.AssignClusterCoordinator(_disaster.Id, 0, _person.Id);

            _dataService.Verify(x => x.AddClusterCoordinator(It.IsAny<ClusterCoordinator>()), Times.Never());
            _dataService.Verify(x => x.AppendClusterCoordinatorLogEntry(It.IsAny<ClusterCoordinatorLogEntry>()), Times.Never());
            Assert.IsNull(result);
        }

        [Test]
        public void AssignClusterCoordinator_with_no_person_id_does_nothing()
        {
            var result = _clusterCoordinatorService.AssignClusterCoordinator(_disaster.Id, _cluster.Id, 0);

            _dataService.Verify(x => x.AddClusterCoordinator(It.IsAny<ClusterCoordinator>()), Times.Never());
            _dataService.Verify(x => x.AppendClusterCoordinatorLogEntry(It.IsAny<ClusterCoordinatorLogEntry>()), Times.Never());
            Assert.IsNull(result);
        }

        [Test]
        public void AssignClusterCoordinator_appends_a_ClusterCoordinatorLogEntry()
        {
            var clusterCoordinator = new ClusterCoordinator
            {
                Id = 1,
                DisasterId = _disaster.Id,
                ClusterId = _cluster.Id,
                PersonId = _person.Id,
                Person = _person,
                Cluster = _cluster,
                Disaster = _disaster
            };
            var coordinatorList = new List<ClusterCoordinator>();

            _dataService.Setup(x => x.ClusterCoordinators).Returns(coordinatorList.AsQueryable());
            _dataService.Setup(
                x => x.AddClusterCoordinator(It.Is<ClusterCoordinator>(cc => cc.DisasterId == _disaster.Id &&
                                                                             cc.ClusterId == _cluster.Id &&
                                                                             cc.PersonId == _person.Id)))
                .Callback(() => coordinatorList.Add(clusterCoordinator));

            _clusterCoordinatorService.AssignClusterCoordinator(clusterCoordinator.DisasterId, clusterCoordinator.ClusterId, clusterCoordinator.PersonId);

            _dataService.Verify(x => x.AddClusterCoordinator(It.IsAny<ClusterCoordinator>()), Times.Once());
            _dataService.Verify(x => x.AppendClusterCoordinatorLogEntry(
                It.Is<ClusterCoordinatorLogEntry>(cc => cc.DisasterId == clusterCoordinator.DisasterId &&
                                                        cc.DisasterName == clusterCoordinator.Disaster.Name &&
                                                        cc.ClusterId == clusterCoordinator.ClusterId &&
                                                        cc.ClusterName == clusterCoordinator.Cluster.Name &&
                                                        cc.PersonId == clusterCoordinator.PersonId &&
                                                        cc.PersonName == clusterCoordinator.Person.FullName &&
                                                        cc.Event == ClusterCoordinatorEvents.Assigned)));
        }

        [Test]
        public void AssignClusterCoordinator_does_not_add_duplicates()
        {
            const int existingClusterCoordinatorId = 8929;

            // Arrange
            _dataService.Setup(x => x.ClusterCoordinators).Returns(
                new EnumerableQuery<ClusterCoordinator>(
                    new[]
                    {
                        new ClusterCoordinator
                        {
                            Id = existingClusterCoordinatorId,
                            DisasterId = _disaster.Id,
                            ClusterId = _cluster.Id,
                            PersonId = _person.Id,
                            Person = _person,
                            Disaster = _disaster,
                            Cluster = _cluster
                        }
                    }));

            // Act
            var result = _clusterCoordinatorService.AssignClusterCoordinator(_disaster.Id, _cluster.Id, _person.Id);

            // Assert
            _dataService.Verify(x => x.AddClusterCoordinator(It.IsAny<ClusterCoordinator>()), Times.Never());
            _dataService.Verify(x => x.AppendClusterCoordinatorLogEntry(It.IsAny<ClusterCoordinatorLogEntry>()), Times.Never());
            Assert.AreEqual(existingClusterCoordinatorId, result.Id);
        }
    }
}