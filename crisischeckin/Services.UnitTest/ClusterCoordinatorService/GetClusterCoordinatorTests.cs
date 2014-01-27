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

        [TestInitialize]
        public void Init()
        {
            _coordinatorId = 42;
            _notCoordinatorId = 10;
            _dataService = new Mock<IDataService>();
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
    }
}