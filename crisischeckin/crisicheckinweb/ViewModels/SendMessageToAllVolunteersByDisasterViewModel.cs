using System;

namespace crisicheckinweb.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Models;

    public class SendMessageToAllVolunteersByDisasterViewModel
    {
        private const int TextMessageLength = 160;

        [Required]
        public int DisasterId { get; set; }
        [StringLength(50)]
        [Display(Name = "Disaster")]
        public string DisasterName { get; set; }
        [Display(Name = "Cluster")]
        public int? ClusterId { get; set; }
        public DateTime VolunteerCommitmentDate { get; set; }
        [Required, StringLength(TextMessageLength)]
        public string Subject { get; set; }
        [Required, StringLength(TextMessageLength * 4)]
        public string Message { get; set; }
        [Display(Name = "Cluster coordinators only")]
        public bool ClusterCoordinatorsOnly { get; set; }
        [Display(Name = "Send to checked-in volunteers only")]
        public bool CheckedInOnly { get; set; }
        [Display(Name = "Send Message via SMS")]
        public bool IsSMSMessage { get; set; }

        public IEnumerable<Cluster> Clusters { get; set; }
    }
}