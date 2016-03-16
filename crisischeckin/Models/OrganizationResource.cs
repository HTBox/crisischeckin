using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class OrganizationResource
    {
        public int OrganizationResourceId { get; set; }

        public Organization Organization { get; set; }

        [MaxLength(15)]
        public string ResourceTitle { get; set; }

        public byte[] Data { get; set; }
    }
}
