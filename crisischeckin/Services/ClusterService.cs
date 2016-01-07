using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (cluster == null) throw new ArgumentNullException("cluster not passed to be created");
            if (String.IsNullOrWhiteSpace(cluster.Name)) throw new ArgumentNullException("cluster name for cluster to be created can not be null");

            _svc.AddCluster(cluster);
        }

        public void Remove(Cluster cluster)
        {
            if (cluster == null) throw new ArgumentNullException("cluster not passed to be deleted");
            if (String.IsNullOrWhiteSpace(cluster.Name)) throw new ArgumentNullException("cluster name for cluster to be deleted can not be null");

            _svc.RemoveCluster(cluster);
        }

 

        public Cluster Update(Cluster updatedCluster)
        {
 
            if (_svc.Clusters.Any(d => d.Name == updatedCluster.Name))
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
