using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrisisCheckinMobile.ViewModels
{
    public class ResourceListItemViewModel
    {
        public int ResourceId { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string PersonFullName { get; set; }
        public string Location_State { get; set; }
        public decimal Qty { get; set; } //TODO - can we have partial quantities, should this be an INT?

        
        //public DateTime StartOfAvailability { get; set; }
        //public DateTime EndOfAvailability { get; set; }
        //public string Location_BuildingName { get; set; }
        //public string Location_AddressLine1 { get; set; }
        //public string Location_AddressLine2 { get; set; }
        //public string Location_AddressLine3 { get; set; }
        //public string Location_City { get; set; }
        //public string Location_County { get; set; }
        //public string Location_State { get; set; }
        //public string Location_Country { get; set; }
        //public string Location_PostalCode { get; set; }
        //public decimal Qty { get; set; } //TODO - can we have partial quantities, should this be an INT?
        //public int Status { get; set; }
        //public int Allocator_OrganizationId { get; set; }
        //public int DisasterId { get; set; }
        //public int ResourceTypeId { get; set; }
        //public int PersonId { get; set; }
        //public DateTime EntryMade { get; set; }
    }
}
