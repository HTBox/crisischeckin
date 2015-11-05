using System.Linq;
using NUnit.Framework;
using Models;
using Moq;
using Services.Interfaces;

namespace Services.UnitTest.ClusterCoordinatorService
{
    [TestFixture]
    public class UnassignClusterCoordinatorTests
    {
        Cluster _cluster;
        ClusterCoordinator _clusterCoordinator;
        Services.ClusterCoordinatorService _clusterCoordinatorService;
        Mock<IDataService> _dataService;
        Disaster _disaster;
        Person _person;

        [SetUp]
        public void Init()
        {
            _clusterCoordinator = new ClusterCoordinator
            {
                                      ClusterId = 9,
                                      DisasterId = 12,
                                      PersonId = 15,
                                  };
            _disaster = new Disaster {Id = _clusterCoordinator.DisasterId, IsActive = true, Name = "Sharknado"};
            _cluster = new Cluster {Id = _clusterCoordinator.ClusterId, Name = "Red Zone"};
            _person = new Person {Id = _clusterCoordinator.PersonId, FirstName = "John", LastName = "Doe"};
            _clusterCoordinator.Person = _person;
            _clusterCoordinator.Disaster = _disaster;
            _clusterCoordinator.Cluster = _cluster;

            _dataService = new Mock<IDataService>();
            _dataService.Setup(x => x.Disasters).Returns(new EnumerableQuery<Disaster>(new[] {_disaster}));
            _dataService.Setup(x => x.Clusters).Returns(new EnumerableQuery<Cluster>(new[] {_cluster}));
            _dataService.Setup(x => x.Persons).Returns(new EnumerableQuery<Person>(new[] {_person}));

            _clusterCoordinatorService = new Services.ClusterCoordinatorService(_dataService.Object);
        }

        [Test]
        public void UnassignClusterCoordinator_removes_an_existing_ClusterCoordinatorRecord()
        {
            _clusterCoordinatorService.UnassignClusterCoordinator(_clusterCoordinator);

            _dataService.Verify(x => x.RemoveClusterCoordinator(It.Is<ClusterCoordinator>(cc => cc.DisasterId == _clusterCoordinator.DisasterId &&
                                                                                                cc.ClusterId == _clusterCoordinator.ClusterId &&
                                                                                                cc.PersonId == _clusterCoordinator.PersonId)));
        }

        [Test]
        public void UnassignClusterCoordinator_appends_a_ClusterCoordinatorLogEntry()
        {
            _clusterCoordinatorService.UnassignClusterCoordinator(_clusterCoordinator);

            _dataService.Verify(x => x.AppendClusterCoordinatorLogEntry(It.Is<ClusterCoordinatorLogEntry>(cc => cc.DisasterId == _clusterCoordinator.DisasterId &&
                                                                                                                cc.DisasterName == _disaster.Name &&
                                                                                                                cc.ClusterId == _clusterCoordinator.ClusterId &&
                                                                                                                cc.ClusterName == _cluster.Name &&
                                                                                                                cc.PersonId == _clusterCoordinator.PersonId &&
                                                                                                                cc.PersonName == _person.FullName &&
                                                                                                                cc.Event == ClusterCoordinatorEvents.Unassigned)));
        }
    }
}