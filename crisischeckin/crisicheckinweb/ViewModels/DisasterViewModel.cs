using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace crisicheckinweb.ViewModels
{
    public class DisasterViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Disaster name")]
        [RegularExpression("^[A-Za-z0-9,. ]*$", ErrorMessageResourceName = "InvalidDisasterName", ErrorMessageResourceType=typeof(Resources.DefaultErrorMessages))]
        public string Name { get; set; }

        [DisplayName("Currently active")]
        public bool IsActive { get; set; }

        public List<SelectedDisasterCluster> SelectedDisasterClusters { get; set; }
    }
}