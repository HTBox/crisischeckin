using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Organization
    {
        public int OrganizationId { get; set; }

        [DisplayName("Organization Name")]
        [MaxLength(50)]
        public string OrganizationName { get; set; }

        public Address Location { get; set; }

        public OrganizationTypeEnum Type { get; set; }
        
        public bool Verified { get; set; }

        // I also haven't implemented the link to the Contacts table yet, because again, we don't have one. 

        public virtual IList<Disaster> Disasters { get; set; }

        public virtual IList<Resource> Resources { get; set; }

        public virtual IList<Person> Persons { get; set; }

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
