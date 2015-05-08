using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class ClusterCoordinatorLogEntry
    {
        public int Id { get; set; }
        public DateTime TimeStampUtc { get; set; }
        public ClusterCoordinatorEvents Event { get; set; }
        public int PersonId { get; set; }
        [StringLength(61)] //30 first name + space + 30 last name
        public string PersonName { get; set; }
        public int DisasterId { get; set; }
        [StringLength(50)]
        public string DisasterName { get; set; }
        public int ClusterId { get; set; }
        [StringLength(50)]
        public string ClusterName { get; set; }
    }
}