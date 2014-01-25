using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;
using Services.Interfaces;

namespace Services.UnitTest.ClusterCoordinatorService
{
    [TestClass]
    public class AssignClusterCoordinatorTests
    {
        Cluster _cluster;
        Services.ClusterCoordinatorService _clusterCoordinatorService;
        Mock<IDataService> _dataService;
        Disaster _disaster;
        Person _person;

        [TestInitialize]
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

        [TestMethod]
        public void AssignClusterCoordinator_saves_a_new_ClusterCoordinatorRecord()
        {
            _clusterCoordinatorService.AssignClusterCoordinator(_disaster.Id, _cluster.Id, _person.Id);

            _dataService.Verify(x => x.AddClusterCoordinator(It.Is<ClusterCoordinator>(cc => cc.DisasterId == _disaster.Id &&
                                                                                             cc.ClusterId == _cluster.Id &&
                                                                                             cc.PersonId == _person.Id)));
        }

        [TestMethod]
        public void AssignClusterCoordinator_appends_a_ClusterCoordinatorLogEntry()
        {
            _clusterCoordinatorService.AssignClusterCoordinator(_disaster.Id, _cluster.Id, _person.Id);

            _dataService.Verify(x => x.AppendClusterCoordinatorLogEntry(It.Is<ClusterCoordinatorLogEntry>(cc => cc.DisasterId == _disaster.Id &&
                                                                                                                cc.DisasterName == _disaster.Name &&
                                                                                                                cc.ClusterId == _cluster.Id &&
                                                                                                                cc.ClusterName == _cluster.Name &&
                                                                                                                cc.PersonId == _person.Id &&
                                                                                                                cc.PersonName == _person.FullName &&
                                                                                                                cc.Event == ClusterCoordinatorEvents.Assigned)));
        }
    }
}