using System.Collections.Generic;
using Models;
using Services.Interfaces;

namespace Services
{
    public class ClusterCoordinatorService : IClusterCoordinatorService
    {
        public void AssignClusterCoordinator(int disasterId, int clusterId, int personId)
        {
            throw new System.NotImplementedException();
        }

        public List<ClusterCoordinator> GetAllCoordinators(int disasterId)
        {
            throw new System.NotImplementedException();
        }
    }
}