namespace Models
{
    public class ClusterCoordinator
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int DisasterId { get; set; }
        public int ClusterId { get; set; }

        public virtual Disaster Disaster { get; set; }
        public virtual Person Coordinator { get; set; }
        public virtual Cluster Cluster { get; set; }
    }
}