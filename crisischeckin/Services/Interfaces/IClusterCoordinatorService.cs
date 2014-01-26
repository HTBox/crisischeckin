using System.Collections.Generic;
using Models;

namespace Services.Interfaces
{
    public interface IClusterCoordinatorService
    {
        List<ClusterCoordinator> GetAllCoordinators(int disasterId);
        ClusterCoordinator AssignClusterCoordinator(int disasterId, int clusterId, int personId);
        void UnassignClusterCoordinator(ClusterCoordinator clusterCoordinator);
    }
}