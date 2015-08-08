using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Services.Interfaces;
using Services.Exceptions;
using Models;

namespace Services
{
    public class ClusterService : ICluster
    {
        private readonly IDataService _svc;
        public ClusterService(IDataService service)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            _svc = service;
        }

        public IEnumerable<Models.Cluster> GetList()
        {
            return _svc.Clusters.OrderBy(c => c.Name).ToList();
        }

        public void Create(Cluster cluster)
        {
            if (cluster == null) throw new ArgumentNullException("cluster");
            if (String.IsNullOrWhiteSpace(cluster.Name)) throw new ArgumentNullException("cluster");
            //if (_dataService.Disasters.Any(d => d.Name == disaster.Name)) throw new DisasterAlreadyExistsException();
            if (_svc.Clusters.Any(d => d.Name == cluster.Name)) throw new ArgumentNullException("cluster already exists");
            // Why should disaster name be unique?


            _svc.AddCluster(cluster);
        }

        public Cluster Update(Cluster updatedCluster)
        {
            if (_svc.Clusters.Count(d => d.Id == updatedCluster.Id) == 0)
                throw new ClusterNotFoundException();

            if (_svc.Clusters.Any(d => d.Name == updatedCluster.Name && d.Id != updatedCluster.Id))
                throw new ClusterAlreadyExistsException();

            var result = _svc.UpdateCluster(updatedCluster);

            return result;
        }

        public Cluster Get(int clusterId)
        {
            return _svc.Clusters.SingleOrDefault(d => d.Id.Equals(clusterId));
        }
    }
}
