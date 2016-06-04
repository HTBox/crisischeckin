using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace crisicheckinweb.ViewModels
{
    public class ClusterGroupViewModel
    {
        
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Cluster Group Name")]
        public string Name { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
    }
}