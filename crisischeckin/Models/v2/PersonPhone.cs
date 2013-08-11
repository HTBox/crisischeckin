using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.v2
{
    public class PersonPhone
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string PhoneNumber { get; set; }
        public int PhoneTypeId { get; set; }

    }
}
