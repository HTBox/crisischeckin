namespace Models
{
    public class ClusterCoordinator
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int DisasterId { get; set; }
        public int ClusterId { get; set; }

        public virtual Person Person { get; set; }
        public virtual Cluster Cluster { get; set; }
    }
}