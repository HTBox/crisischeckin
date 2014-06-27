using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}
