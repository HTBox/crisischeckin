using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Address
    {
        [MaxLength(30)]
        public string BuildingName { get; set; }
        [MaxLength(40)]
        public string AddressLine1 { get; set; }

        [MaxLength(40)]
        public string AddressLine2 { get; set; }

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

        [MaxLength(15)]
        public string PostalCode { get; set; }
    
    }
}
