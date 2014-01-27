using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;
using Services.Interfaces;

namespace Services.UnitTest.ClusterCoordinatorService
{
    [TestClass]
    public class GetClusterCoordinatorsTests
    {
        Services.ClusterCoordinatorService _clusterCoordinatorService;
        Mock<IDataService> _dataService;
        int _disasterId;
        int _notDisasterId;

        [TestInitialize]
        public void Init()
        {
            _disasterId = 4;
            _notDisasterId = 5;

            _dataService = new Mock<IDataService>();
            _clusterCoordinatorService = new Services.ClusterCoordinatorService(_dataService.Object);
        }

        [TestMethod]
        public void GetClusterCoordinators_return_only_the_ClusterCoordinators_for_the_specified_disaster()
        {
            var coordinators = new EnumerableQuery<ClusterCoordinator>(new[]
                                                                       {
                                                                           new ClusterCoordinator {DisasterId = _disasterId},
                                                                           new ClusterCoordinator {DisasterId = _disasterId},
                                                                           new ClusterCoordinator {DisasterId = _notDisasterId},
                                                                           new ClusterCoordinator {DisasterId = _notDisasterId},
                                                                       });
            _dataService.Setup(x => x.ClusterCoordinators).Returns(coordinators);

            var clusterCoordinators = _clusterCoordinatorService.GetAllCoordinators(_disasterId);

            Assert.IsTrue(clusterCoordinators.All(cc => cc.DisasterId == _disasterId));
        }
    }
}