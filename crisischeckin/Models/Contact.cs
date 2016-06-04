using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Contact
    {
        public int ContactId { get; set; }

        [MaxLength(15)]
        public string Title { get; set; }

        public Organization Organization { get; set; }

        public Person Person { get; set; }
    }
}
