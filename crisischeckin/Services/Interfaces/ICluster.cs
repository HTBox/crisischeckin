using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICluster
    {
        IEnumerable<Cluster> GetList();
        void Create(Cluster cluster);
        void Remove(Cluster cluster);
        Cluster Update(Cluster cluster);
        Cluster Get(int clusterId);
    }
}
