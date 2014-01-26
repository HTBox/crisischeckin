using System.Collections.Generic;
using Models;

namespace Services.Interfaces
{
    public interface IClusterCoordinatorService
    {
        List<ClusterCoordinator> GetAllCoordinators(int disasterId);
        void AssignClusterCoordinator(int disasterId, int clusterId, int personId);
        void UnassignClusterCoordinator(int disasterId, int clusterId, int personId);
    }
}