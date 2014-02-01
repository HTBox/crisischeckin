using System.Collections.Generic;
using System.ComponentModel;
using Models;

namespace crisicheckinweb.ViewModels
{
    public class DisasterClusterCoordinatorsViewModel
    {
        public int DisasterId { get; set; }
        
        [DisplayName("Available Clusters")]
        public List<Cluster> AvailableClusters { get; set; }
        
        [DisplayName("Available People")]
        public List<Person> AvailablePeople { get; set; }
        
        public string DisasterName { get; set; }
        
        public List<ClusterViewModel> Clusters { get; set; }
        
        public int SelectedClusterId { get; set; }
        
        public int SelectedPersonId { get; set; }
    }
}