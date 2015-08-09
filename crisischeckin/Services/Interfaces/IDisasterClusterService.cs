using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IDisasterClusterService
    {
        List<DisasterCluster> GetClustersForADisaster(int disasterId);
        void Create(DisasterCluster disasterCluster);
        void Remove(DisasterCluster disasterCluster);
    }
}