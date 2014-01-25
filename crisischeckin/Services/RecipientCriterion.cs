namespace Services
{
    public class RecipientCriterion
    {
        public int DisasterId { get; private set; }
        public int? ClusterId { get; private set; }

        public RecipientCriterion(int disasterId, int? clusterId = null)
        {
            DisasterId = disasterId;
            ClusterId = clusterId;
        }
    }
}
