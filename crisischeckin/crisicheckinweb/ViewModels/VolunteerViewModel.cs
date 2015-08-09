using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Models;

namespace crisicheckinweb.ViewModels
{
    public class VolunteerViewModel
    {
        public IEnumerable<Disaster> Disasters { get; set; }
        public IEnumerable<DisasterCluster> DisasterClusters { get; set; }
        public IEnumerable<Commitment> MyCommitments { get; set; }
        public int RemoveCommitmentId { get; set; }
        public int PersonId { get; set; }
        [DisplayName("Volunteer for Disaster")]
        public int SelectedDisasterId { get; set; }
        [DisplayName("Activity")]
        public int SelectedClusterId { get; set; }
        [DisplayName("Start Date")]
        public DateTime SelectedStartDate { get; set; }
        [DisplayName("End Date")]
        public DateTime SelectedEndDate { get; set; }
        [DisplayName("Location")]
        public int VolunteerType { get; set; }
        public IEnumerable<VolunteerType> VolunteerTypes { get; set; }
        public Person Person { get; set; }
        public IEnumerable<ClusterCoordinator> ClusterCoordinators { get; set; }
    }
}