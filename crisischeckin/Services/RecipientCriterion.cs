namespace Services
{
    using System.Collections.Generic;
    public class RecipientCriterion
    {
        public int DisasterId { get; private set; }
        public int? ClusterId { get; private set; }

        public IEnumerable<int> ClusterIds { get; private set; }

        public bool ClusterCoordinatorsOnly { get; private set; }
        public bool CheckedInOnly { get; private set; }

        public RecipientCriterion(int disasterId, IEnumerable<int> clusterIds = null, bool clusterCoordinatorsOnly = false, bool checkedInOnly = false)
        {
            DisasterId = disasterId;
            ClusterIds = clusterIds;
            ClusterCoordinatorsOnly = clusterCoordinatorsOnly;
            CheckedInOnly = checkedInOnly;
        }
    }
}
