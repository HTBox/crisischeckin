using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Request
    {
        public int RequestId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public int OrganizationId { get; set; }
        public int CreatorId { get; set; }
        public bool Completed { get; set; }
        public string Location { get; set; }
        public List<RequestAssigniee> Assigniees { get; set; } 

        public virtual Organization Organization { get; set; }
        public virtual Person Creator { get; set; }
        public virtual List<RequestAssigniee> Assignees { get; set; }

    }
}
