using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace crisicheckinweb.ViewModels
{
    public class ClusterViewModel
    {
        
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Cluster Name")]
        public string Name { get; set; }

        public List<ClusterCoordinatorViewModel> Coordinators { get; set; }
    }
}