using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace crisicheckinweb.ViewModels
{
    public class ClusterViewModel
    {
        [StringLength(50)]
        public string Name { get; set; }
        public List<ClusterCoordinatorViewModel> Coordinators { get; set; }
    }
}