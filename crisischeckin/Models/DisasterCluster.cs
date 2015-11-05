namespace Models
{
    public class DisasterCluster
    {
        public int Id { get; set; }

        public int ClusterId { get; set; }

        public int DisasterId { get; set; }

        public virtual Disaster Disaster { get; set; }

        public virtual Cluster Cluster { get; set; }
    }
}