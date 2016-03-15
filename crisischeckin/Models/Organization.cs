using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Organization
    {
        public int OrganizationId { get; set; }

        [MaxLength(50)]
        public string OrganizationName { get; set; }

        public Address Location { get; set; }

        public OrganizationTypeEnum Type { get; set; }
        
        // I haven't implemented the link to the resources table yet, because we don't have one.
        // I also haven't implemented the link to the Contacts table yet, because again, we don't have one. 

        public virtual Person PocPerson { get; set; }

        public virtual IList<Disaster> Disasters { get; set; }
        
    }

    public enum OrganizationTypeEnum
    {
        NonProfit = 0,
        NGO = 1,
        Private = 2,
        State = 3,
        Local = 4
    }
}
