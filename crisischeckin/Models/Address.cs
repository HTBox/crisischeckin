using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Address
    {
        [DisplayName("Building Name")]
        [MaxLength(30)]
        public string BuildingName { get; set; }

        [DisplayName("Address Line 1")]
        [MaxLength(40)]
        public string AddressLine1 { get; set; }

        [DisplayName("Address Line 2")]
        [MaxLength(40)]
        public string AddressLine2 { get; set; }

        [DisplayName("Address Line 3")]
        [MaxLength(40)]
        public string AddressLine3 { get; set; }

        [MaxLength(30)]
        public string City { get; set; }

        [MaxLength(30)]
        public string County { get; set; }

        [MaxLength(30)]
        public string State { get; set; }

        [MaxLength(30)]
        public string Country { get; set; }

        [DisplayName("Postal Code")]
        [MaxLength(15)]
        public string PostalCode { get; set; }
    
    }
}
