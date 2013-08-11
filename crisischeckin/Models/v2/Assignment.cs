using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.v2
{
    public class Assignment
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public int PersonId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


    }
}
