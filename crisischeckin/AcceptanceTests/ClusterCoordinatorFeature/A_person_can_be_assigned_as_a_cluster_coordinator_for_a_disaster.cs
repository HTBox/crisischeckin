using System.Linq;
using Models;
using NUnit.Framework;
using Services;
using Services.Interfaces;

namespace AcceptanceTests.ClusterCoordinatorFeature
{
    [TestFixture]
    public class A_person_can_be_assigned_as_a_cluster_coordinator_for_a_disaster : With_an_empty_database_environment
    {
        IClusterCoordinatorService _clusterCoordinatorService;
        DataAccessHelper _dataAccessHelper;
        DataService _dataService;
        Disaster _disaster;
        Person _person;
        int _clusterId;

        [SetUp]
        public void Arrange()
        {
            _dataService = new DataService(new CrisisCheckin(), new CrisisCheckinMembership());
            _dataAccessHelper = new DataAccessHelper(_dataService);
            _clusterCoordinatorService = new ClusterCoordinatorService(_dataService);
            _disaster = _dataAccessHelper.Create_a_disaster();
            _person = _dataAccessHelper.Create_a_volunteer();
            _clusterId = _person.ClusterId.GetValueOrDefault();
        }

        [Test]
        public void Assign_a_user_and_verify_results()
        {
            _clusterCoordinatorService.AssignClusterCoordinator(_disaster.Id, _clusterId, _person.Id);

            var clusterCoordinators = _clusterCoordinatorService.GetAllCoordinators(_disaster.Id);

            Assert.IsTrue(clusterCoordinators.Any(c => c.DisasterId == _disaster.Id && c.ClusterId == _clusterId && c.PersonId == _person.Id));
            Assert.IsTrue(_dataService.ClusterCoordinatorLogEntries.Any(c => c.DisasterId == _disaster.Id &&
                                                                             c.ClusterId == _clusterId &&
                                                                             c.PersonId == _person.Id &&
                                                                             c.Event == ClusterCoordinatorEvents.Assigned));
        }
    }
}