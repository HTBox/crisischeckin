using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Resource
    {
        public int ResourceId { get; set; }

        public Organization Allocator { get; set; }
        public virtual Person Person { get; set; }
        public int? PersonId { get; set; }
        public string Description { get; set; }

        public DateTime EntryMade { get; set; }

        public DateTime StartOfAvailability { get; set; }
        public DateTime EndOfAvailability { get; set; }
        public Address Location { get; set; }
        public Decimal Qty { get; set; }
        public ResourceStatus Status { get; set; }

        public int DisasterId { get; set; }
        public virtual Disaster Disaster { get; set; }

        public int ResourceTypeId { get; set; }
        public virtual ResourceType ResourceType { get; set; }      
         
    }

    public enum ResourceStatus
    {
        Available = 1,
        EnRoute = 2,
        Delivered = 3
    }
}
