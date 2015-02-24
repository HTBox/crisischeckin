namespace Services
{
    public class RecipientCriterion
    {
        public int DisasterId { get; private set; }
        public int? ClusterId { get; private set; }

        public bool ClusterCoordinatorsOnly { get; private set; }
        public bool CheckedInOnly { get; private set; }

        public RecipientCriterion(int disasterId, int? clusterId = null, bool clusterCoordinatorsOnly = false, bool checkedInOnly = false)
        {
            DisasterId = disasterId;
            ClusterId = clusterId;
            ClusterCoordinatorsOnly = clusterCoordinatorsOnly;
            CheckedInOnly = checkedInOnly;
        }
    }
}
