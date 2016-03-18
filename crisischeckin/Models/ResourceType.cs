using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class ResourceType
    {
        public int ResourceTypeId { get; set; }

        public string TypeName { get; set; }
    }
}
