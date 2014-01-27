using System.Collections.Generic;

namespace crisicheckinweb.ViewModels
{
    public class ClusterViewModel
    {
        public string Name { get; set; }
        public List<ClusterCoordinatorViewModel> Coordinators { get; set; }
    }
}