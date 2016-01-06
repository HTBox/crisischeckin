using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IClusterGroup
    {
        IEnumerable<ClusterGroup> GetList();
        void Create(ClusterGroup cluster);
        void Remove(ClusterGroup cluster);
        ClusterGroup Update(ClusterGroup cluster);
        ClusterGroup Get(int clusterId);
    }
}
