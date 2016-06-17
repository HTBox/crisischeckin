using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class ResourceType
    {
        public int ResourceTypeId { get; set; }
        [DisplayName("Type")]
        public string TypeName { get; set; }
    }
}
