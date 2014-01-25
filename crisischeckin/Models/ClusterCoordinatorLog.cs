using System;

namespace Models
{
    public class ClusterCoordinatorLogEntry
    {
        public int Id { get; set; }
        public DateTime TimeStampUtc { get; set; }
        public ClusterCoordinatorEvents Event { get; set; }
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public int DisasterId { get; set; }
        public string DisasterName { get; set; }
        public int ClusterId { get; set; }
        public string ClusterName { get; set; }
    }
}