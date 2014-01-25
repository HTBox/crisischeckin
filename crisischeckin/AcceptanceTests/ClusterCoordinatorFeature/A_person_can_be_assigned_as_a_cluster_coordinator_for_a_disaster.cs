using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Services;
using Services.Interfaces;

namespace AcceptanceTests.ClusterCoordinatorFeature
{
    [TestClass]
    public class A_person_can_be_assigned_as_a_cluster_coordinator_for_a_disaster : With_an_empty_database_environment
    {
         int _clusterId = 5;
        const int UserId = 100;
        Disaster _disaster;
        Person _person;
        DataService _dataService;

        [TestInitialize]
        public void Arrange()
        {
            Create_a_disaster();
            Create_a_person();
        }

        [TestMethod]
        public void Assign_a_user_and_verify_results()
        {
            IClusterCoordinatorService clusterCoordinatorService = new ClusterCoordinatorService();
            clusterCoordinatorService.AssignClusterCoordinator(_disaster.Id, _clusterId, _person.Id);

            var clusterCoordinators = clusterCoordinatorService.GetAllCoordinators(_disaster.Id);

            Assert.IsTrue(clusterCoordinators.Any(c => c.DisasterId == _disaster.Id && c.ClusterId == _clusterId && c.PersonId == _person.Id));
            //Assert that the log contains an assigned event
        }

        void Create_a_person()
        {
            _dataService = new DataService(new CrisisCheckin());
            var volunteerService = new VolunteerService(_dataService);
            _person = volunteerService.Register("Rob", "Lowe", "rob@lowe.com", "890-1230-4567", _clusterId, UserId);
        }

        void Create_a_disaster()
        {
            _disaster = new Disaster
                        {
                            IsActive = true,
                            Name = "Great Seattle Starbucks Strike",
                        };
            _clusterId = _dataService.Clusters.First().Id;
        }
    }
}