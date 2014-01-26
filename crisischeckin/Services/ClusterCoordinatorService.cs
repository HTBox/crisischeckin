using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Services.Interfaces;

namespace Services
{
    public class ClusterCoordinatorService : IClusterCoordinatorService
    {
        readonly IDataService dataService;

        public ClusterCoordinatorService(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public void AssignClusterCoordinator(int disasterId, int clusterId, int personId)
        {
            dataService.AddClusterCoordinator(new ClusterCoordinator {DisasterId = disasterId, ClusterId = clusterId, PersonId = personId});
            var clusterCoordinatorLogEntry = new ClusterCoordinatorLogEntry
                                             {
                                                 Event = ClusterCoordinatorEvents.Assigned,
                                                 TimeStampUtc = DateTime.UtcNow,
                                                 ClusterId = clusterId,
                                                 ClusterName = dataService.Clusters.Single(x=>x.Id == clusterId).Name,
                                                 DisasterId = disasterId,
                                                 DisasterName = dataService.Disasters.Single(x=>x.Id == disasterId).Name,
                                                 PersonId = personId,
                                                 PersonName = dataService.Persons.Single(x=>x.Id == personId).FullName,
                                             };
            dataService.AppendClusterCoordinatorLogEntry(clusterCoordinatorLogEntry);
        }

        public List<ClusterCoordinator> GetAllCoordinators(int disasterId)
        {
            return dataService.ClusterCoordinators.Where(x => x.DisasterId == disasterId).ToList();
        }
    }
}