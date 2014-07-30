using System.Collections.Generic;
using Models;

namespace crisicheckinweb.ViewModels
{
    public class DisasterClusterCoordinatorsViewModel
    {
        public int DisasterId { get; set; }
        public List<Cluster> AvailableClusters { get; set; }
        public IList<Person> AvailablePeople { get; set; }
        public string DisasterName { get; set; }
        public List<ClusterViewModel> Clusters { get; set; }
        public int SelectedClusterId { get; set; }
        public int SelectedPersonId { get; set; }
    }
}