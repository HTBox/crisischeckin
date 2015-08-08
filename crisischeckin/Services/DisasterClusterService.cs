using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DisasterClusterService : IDisasterClusterService
    {
        private readonly IDataService _svc;

        public DisasterClusterService(IDataService service)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            _svc = service;
        }

        public List<DisasterCluster> GetClustersForADisaster(int disasterId)
        {
            return _svc.DisasterClusters.Where(x => x.DisasterId == disasterId).OrderBy(x => x.Cluster.Name).ToList();
        }
    }
}
