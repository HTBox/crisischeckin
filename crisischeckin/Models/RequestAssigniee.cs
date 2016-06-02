using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RequestAssigniee
    {
        public int AssignieeId { get; set; }
        public int RequestId { get; set; }

        public virtual Person Assigniee { get; set; }
        public virtual Request Request { get; set; }
    }
}
