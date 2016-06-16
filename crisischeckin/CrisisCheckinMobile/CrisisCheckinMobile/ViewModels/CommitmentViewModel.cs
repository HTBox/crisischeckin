using System;
using CrisisCheckinMobile.Models;

namespace CrisisCheckinMobile.ViewModels
{
	public class CommitmentViewModel
	{
        public int Id { get; set; }
        public int PersonId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DisasterId { get; set; }
        //public int VolunteerTypeId { get; set; }
        public bool PersonIsCheckedIn { get; set; }
        public CommitmentStatus Status { get; set; }
	}
}
