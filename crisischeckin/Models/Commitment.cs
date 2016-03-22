using System;
using System.Linq;

namespace Models
{
    public class Commitment
    {
        public int Id { get; set; }

        public int PersonId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int DisasterId { get; set; }

        public int VolunteerTypeId { get; set; }

        public CommitmentStatus Status { get; set; }

        public int? ClusterId { get; set; }

        public virtual Disaster Disaster { get; set; }

        public virtual Person Person { get; set; }

        public virtual Cluster Cluster { get; set; }

        public virtual VolunteerType VolunteerType { get; set; }

        public virtual string Location { get; set; }



        public bool PersonIsCheckedIn
        {
            get { return Status == CommitmentStatus.Here; }
        }

        public void CheckIn()
        {
            Status = CommitmentStatus.Here;
        }

        public void ReportDelay(DateTime startDate, DateTime endDate)
        {
            Status = CommitmentStatus.Delayed;
            StartDate = startDate;
            EndDate = endDate;
        }

        public void ReportUnavailable()
        {
            Status = CommitmentStatus.Unavailable;
        }

        public void CheckOut()
        {
            Status = CommitmentStatus.Finished;
        }

        public static IQueryable<Commitment> FilteredByStatus(IQueryable<Commitment> commitments, bool checkedInOnly)
        {
            if (checkedInOnly)
                return commitments.Where(c => c.Status == CommitmentStatus.Here);
            else
                return commitments;
        }
    }

    public enum CommitmentStatus
    {
        Planned = 0,
        Delayed = 1,
        Here = 2,
        Unavailable = 3,
        Finished = 4
    }
}