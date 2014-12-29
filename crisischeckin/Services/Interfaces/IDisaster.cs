using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace Services.Interfaces
{
    public interface IDisaster
    {
        void AssignToVolunteer(int disasterId, int personId, DateTime startDate, DateTime endDate);
        void RemoveCommitmentById(int commitmentId);
        Disaster Get(int disasterId);
        void Create(Disaster disaster);
        Disaster Update(Disaster updatedDisaster);
        IEnumerable<Disaster> GetActiveList();
        IEnumerable<Disaster> GetList();
        string GetName(int disasterId);
    }
}
