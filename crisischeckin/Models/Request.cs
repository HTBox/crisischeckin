using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Request
    {
        public int RequestId { get; set; }
        [DisplayName("Created On")]
        public DateTime CreatedDate { get; set; }
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public int CreatorId { get; set; }
        [DisplayName("Status")]
        public bool Completed { get; set; }
        public string Location { get; set; }
        public List<RequestAssigniee> Assigniees { get; set; } 

        public virtual Person Creator { get; set; }
        public virtual List<RequestAssigniee> Assignees { get; set; }

    }
}
