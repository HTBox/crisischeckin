using System;

namespace CrisisCheckinMobile.Models
{
    public class CommitmentDto
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DisasterId { get; set; }
        public bool PersonIsCheckedIn { get; set; }
        public CommitmentStatus Status { get; set; }

        public DisasterDto Disaster { get; set; }       
    }

    public enum CommitmentStatus // TODO: Commented-out the previously-used values. They don't line up with the data model enum. Using the data model version for now
    {
        Planned = 0,
        Delayed = 1,
        Here = 2,
        Unavailable = 3,
        Finished = 4
        //None,
        //Planned,
        //Out,
        //Here,
        //Unavailable,
        //Finished
    }
}
