using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models;

namespace Models
{
    public class Resource
    {
        public int ResourceId { get; set; }

        public Organization Allocator { get; set; }
        public virtual Person Person { get; set; }
        public int? PersonId { get; set; }
        public string Description { get; set; }
        [DisplayName("Date Created")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}",
               ApplyFormatInEditMode = true)]
        public DateTime EntryMade { get; set; }
        [DisplayName("Availability Start")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}",
               ApplyFormatInEditMode = true)]
        public DateTime StartOfAvailability { get; set; }
        [DisplayName("Availability End")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}",
               ApplyFormatInEditMode = true)]
        public DateTime EndOfAvailability { get; set; }
        public Address Location { get; set; }
        [DisplayName("Quantity")]
        public Decimal Qty { get; set; }
        public ResourceStatus Status { get; set; }

        [DisplayName("Disaster")]
        public int DisasterId { get; set; }
        public virtual Disaster Disaster { get; set; }
        [DisplayName("Type")]
        public int ResourceTypeId { get; set; }
        public virtual ResourceType ResourceType { get; set; }      
         
    }

    public enum ResourceStatus
    {
        All = 0,
        Available = 1,
        EnRoute = 2,
        Delivered = 3,
        
    }
}