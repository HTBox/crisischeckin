using System;

namespace Models
{
    public class Commitment
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DisasterId { get; set; }
        public bool PersonIsCheckedIn { get; set; }
        public CommitmentStatus Status { get; set; }

        public virtual Disaster Disaster { get; set; }
        public virtual Person Person { get; set; }
    }

    public enum CommitmentStatus
    {
        None,
        Planned,
        Out,
        Here,
        Unavailable,
        Finished
    }
}
