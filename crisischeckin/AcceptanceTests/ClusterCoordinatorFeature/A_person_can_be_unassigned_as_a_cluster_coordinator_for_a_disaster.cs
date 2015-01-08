using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Services;
using Services.Interfaces;

namespace AcceptanceTests.ClusterCoordinatorFeature
{
    [TestClass]
    public class A_person_can_be_unassigned_as_a_cluster_coordinator_for_a_disaster : With_an_empty_database_environment
    {
        ClusterCoordinator _clusterCoordinator;
        IClusterCoordinatorService _clusterCoordinatorService;
        DataAccessHelper _dataAccessHelper;
        DataService _dataService;

        [TestInitialize]
        public void Arrange()
        {
            _dataService = new DataService(new CrisisCheckin(), new CrisisCheckinMembership());
            _dataAccessHelper = new DataAccessHelper(_dataService);
            _clusterCoordinatorService = new ClusterCoordinatorService(_dataService);
            var disaster = _dataAccessHelper.Create_a_disaster();
            var person = _dataAccessHelper.Create_a_volunteer();
            var clusterId = person.ClusterId.GetValueOrDefault();
            _clusterCoordinator = _clusterCoordinatorService.AssignClusterCoordinator(disaster.Id, clusterId, person.Id);
        }

        [TestMethod]
        public void Unassign_a_user_and_verify_results()
        {
            _clusterCoordinatorService.UnassignClusterCoordinator(_clusterCoordinator);

            var clusterCoordinators = _clusterCoordinatorService.GetAllCoordinators(_clusterCoordinator.DisasterId);

            Assert.IsFalse(clusterCoordinators.Any(c => c.DisasterId == _clusterCoordinator.DisasterId &&
                                                        c.ClusterId == _clusterCoordinator.ClusterId &&
                                                        c.PersonId == _clusterCoordinator.PersonId));
            Assert.IsTrue(_dataService.ClusterCoordinatorLogEntries.Any(c => c.DisasterId == _clusterCoordinator.DisasterId &&
                                                                             c.ClusterId == _clusterCoordinator.ClusterId &&
                                                                             c.PersonId == _clusterCoordinator.PersonId &&
                                                                             c.Event == ClusterCoordinatorEvents.Unassigned));
        }
    }
}