using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Api.Requests
{
    public class ClustersRequest : ServiceRequest
    {
        public override string ServiceMethodUrl { get { return "clusters"; } }
    }
}
