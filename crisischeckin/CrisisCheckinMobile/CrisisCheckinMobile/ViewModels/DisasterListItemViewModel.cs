using System;
using CrisisCheckinMobile.Models;

namespace CrisisCheckinMobile.ViewModels
{
    public class DisasterListItemViewModel
    {
        private readonly string _disasterName;
        private readonly string _disasterStatusAndDate;
        private readonly int _id;

        public DisasterListItemViewModel(CommitmentDto dto)
        {
            _id = dto.Disaster.Id;
            _disasterName = dto.Disaster.Name;
            _disasterStatusAndDate = string.Format("{0} - until {1}", 
                Enum.GetName(typeof(CommitmentStatus), dto.Status), dto.EndDate.ToString("MMMM dd, yyyy"));

            CommitmentData = new CommitmentViewModel
            {
                Id = dto.Id,
                PersonId = dto.PersonId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                DisasterId = dto.Disaster.Id,
                Status = dto.Status
            };
        }

        public CommitmentViewModel CommitmentData { get; set; }

        public string DisasterName
        {
            get
            {
                return _disasterName;
            }
        }

        public string DisasterStatusAndDate
        {
            get
            {
                return _disasterStatusAndDate;
            }
        }
    }
}