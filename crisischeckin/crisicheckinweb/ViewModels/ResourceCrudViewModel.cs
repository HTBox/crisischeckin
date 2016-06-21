using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace crisicheckinweb.ViewModels
{
    public class ResourceCrudViewModel
    {
        [DisplayName("Resource Id")]
        public int ResourceId { get; set; }

        public int? PersonId { get; set; }

        public string Description { get; set; }

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

        [DisplayName("Type")]
        public int ResourceTypeId { get; set; }

        [DisplayName("Organization")]
        public int? SelectedOrganizationId { get; set; }

        [DisplayName("Date Created")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}",
               ApplyFormatInEditMode = true)]
        public DateTime EntryMade { get; set; }


        [DisplayName("Disaster")]
        public Disaster Disaster { get; set; }
        [DisplayName("Person")]
        public Person Person { get; set; }
        [DisplayName("Organization")]
        public Organization Allocator { get; set; }
        [DisplayName("Type")]
        public ResourceType ResourceType { get; set; }


        public int? FixedDisasterId { get; set; }
        public Disaster FixedDisaster { get; set; }
        public int? FixedOrganizationId { get; set; }
        public Organization FixedOrganization { get; set; }
    }
}