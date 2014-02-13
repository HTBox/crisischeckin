using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace crisicheckinweb.ViewModels
{
    public class VolunteerViewModel
    {
        public IEnumerable<Disaster> Disasters { get; set; }
        public IEnumerable<Commitment> MyCommitments { get; set; }
        public int RemoveCommitmentId { get; set; }
        public int PersonId { get; set; }
        public int SelectedDisasterId { get; set; }
        public DateTime SelectedStartDate { get; set; }
        public DateTime SelectedEndDate { get; set; }
    }
}