using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    internal class DisasterCluster
    {
        public int Id { get; set; }

        public int ClusterId { get; set; }

        public int DisasterId { get; set; }

        public virtual Disaster Disaster { get; set; }

        public virtual Cluster Cluster { get; set; }
    }
}