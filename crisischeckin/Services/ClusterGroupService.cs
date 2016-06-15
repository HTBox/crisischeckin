using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Services.Exceptions;

namespace Services
{
    public class ClusterGroupService : IClusterGroup
    {
        private readonly IDataService _svc;

        public ClusterGroupService(IDataService service)
        {
            if (service == null) { throw new ArgumentNullException("service"); }
            _svc = service;
        }

        public void Create(ClusterGroup clusterGroup)
        {
            if (clusterGroup == null) throw new ArgumentNullException("cluster not passed to be created");
            if (String.IsNullOrWhiteSpace(clusterGroup.Name)) throw new ArgumentNullException("cluster name for cluster to be created can not be null");

            _svc.AddClusterGroup(clusterGroup);
        }

        public ClusterGroup Get(int clusterGroupId)
        {
            return _svc.ClusterGroups.SingleOrDefault(d => d.Id.Equals(clusterGroupId));
        }

        public IEnumerable<ClusterGroup> GetList()
        {
            return _svc.ClusterGroups.OrderBy(c => c.Name).ToList();
        }

        public void Remove(ClusterGroup clusterGroup)
        {
            if (clusterGroup == null) throw new ArgumentNullException("cluster not passed to be deleted");
            if (String.IsNullOrWhiteSpace(clusterGroup.Name)) throw new ArgumentNullException("cluster name for cluster to be deleted can not be null");

            _svc.RemoveClusterGroup(clusterGroup);
        }

        public ClusterGroup Update(ClusterGroup clusterGroup)
        {
            if (_svc.Clusters.Any(d => d.Name == clusterGroup.Name))
                throw new ClusterGroupAlreadyExistsException();

            var result = _svc.UpdateClusterGroup(clusterGroup);

            return result;
        }
    }
}
