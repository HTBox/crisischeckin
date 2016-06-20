using System;
using CrisisCheckinMobile.Models;

namespace CrisisCheckinMobile.ViewModels
{
    public class DisasterViewModel
    {
        public DisasterViewModel(DisasterDto dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            IsActive = dto.IsActive;
        }

        public CommitmentViewModel CommitmentData { get; set; }

        public string Name
        {
            get;
            set;
        }

        public int Id { get; set; }

        public bool IsActive { get; set; }
    }
}