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
        public string DisasterName { get; set; }
        [Display(Name="Cluster")]
        public int? ClusterId { get; set; }
        public DateTime VolunteerCommitmentDate { get; set; }
        [Required, StringLength(TextMessageLength)]
        public string Subject { get; set; }
        [Required, StringLength(TextMessageLength*4)]
        public string Message { get; set; }

        public IEnumerable<Cluster> Clusters { get; set; }
    }
}