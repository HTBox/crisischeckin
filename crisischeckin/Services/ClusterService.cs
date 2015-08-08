using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}
