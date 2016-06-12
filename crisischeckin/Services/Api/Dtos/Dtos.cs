using System;
using Models;

namespace Services.Api.Dtos
{
    #region Dtos
    public class DisasterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Location { get; set; }
    }

    public class ClusterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CommitmentDto
    {
        public int DisasterId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int PersonId { get; set; }
        public CommitmentStatus Status { get; set; }

        public DisasterDto Disaster { get; set; }
    }
    #endregion
}
