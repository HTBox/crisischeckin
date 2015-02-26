using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Models;

namespace crisicheckinweb.ViewModels
{
    public class VolunteerViewModel : IValidatableObject
    {
        public IEnumerable<Disaster> Disasters { get; set; }
        public IEnumerable<Commitment> MyCommitments { get; set; }
        public int RemoveCommitmentId { get; set; }
        public int PersonId { get; set; }
        [DisplayName("Volunteer for Disaster")]
        public int SelectedDisasterId { get; set; }
        [DisplayName("Start Date")]
        public DateTime SelectedStartDate { get; set; }
        [DisplayName("End Date")]
        public DateTime SelectedEndDate { get; set; }
        [DisplayName("Location")]
        public int VolunteerType { get; set; }
        public IEnumerable<VolunteerType> VolunteerTypes { get; set; }
        public Person Person { get; set; }
        public IEnumerable<ClusterCoordinator> ClusterCoordinators { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateTime.Compare(DateTime.Today, SelectedStartDate) > 0)
            {
                yield return new ValidationResult("Please enter a start date that is greater than or equal to today's date.", new [] { "SelectedStartDate" });
            }
        }
    }
}