using System.Collections.Generic;
using Models;

namespace Services.Interfaces
{
    public interface IClusterCoordinatorService
    {
        void AssignClusterCoordinator(int disasterId, int clusterId, int personId);
        List<ClusterCoordinator> GetAllCoordinators(int disasterId);
    }
}