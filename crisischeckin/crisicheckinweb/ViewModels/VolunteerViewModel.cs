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
        public IEnumerable<Resource> MyOrgResources { get; set; }
        public IEnumerable<AvailableAction> AvailableActions { get; set; }
        public int RemoveCommitmentId { get; set; }
        public int RemoveResourceId { get; set; }
        public int PersonId { get; set; }
        [DisplayName("Disaster")]
        public int SelectedDisasterId { get; set; }
        [DisplayName("Activity")]
        public int SelectedClusterId { get; set; }
        [DisplayName("Start Date")]
        public DateTime SelectedStartDate { get; set; }
        [DisplayName("End Date")]
        public DateTime SelectedEndDate { get; set; }
        [DisplayName("Location")]
        public int VolunteerType { get; set; }
        [DisplayName("Location Detail")]
        public string Location { get; set; }

        [DisplayName("Start Date")]
        public DateTime ResourceStartDate { get; set; }
        [DisplayName("End Date")]
        public DateTime ResourceEndDate { get; set; }

        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Quantity")]
        public int Qty { get; set; }
        [DisplayName("Resource Type")]
        public int SelectedResourceTypeId { get; set; }
        public IEnumerable<ResourceType> ResourceTypes { get; set; }

        public IEnumerable<VolunteerType> VolunteerTypes { get; set; }
        public Person Person { get; set; }
        public IEnumerable<ClusterCoordinator> ClusterCoordinators { get; set; }
    }
}