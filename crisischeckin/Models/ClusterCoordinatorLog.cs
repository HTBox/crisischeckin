using System;

namespace Models
{
    public class ClusterCoordinatorLog
    {
        public int Id { get; set; }
        public DateTime TimeStampUtc { get; set; }
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public int DisasterId { get; set; }
        public string DisasterName { get; set; }
        public int ClusterId { get; set; }
        public string ClusterName { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
    }
}