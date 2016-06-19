using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrisisCheckinMobile.Models
{
    public class RequestDto
    {
        public int RequestId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public int CreatorId { get; set; }
        public int? AssigneeId { get; set; }
        public int? OrganizationId { get; set; }

        public bool Completed { get; set; }
        public string Location { get; set; }

        //TODO:
        //public virtual Person Creator { get; set; }
        //public virtual Person Assignee { get; set; }
        //public virtual Organization Organization { get; set; }
    }
}
