using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services.Interfaces
{
    public interface IDisaster
    {
        void AssignToVolunteer(int disasterId, int personId, DateTime startDate, DateTime endDate, 
            int volunteerType, int clusterId);
        void RemoveCommitmentById(int commitmentId);
        Disaster Get(int disasterId);
        void Create(Disaster disaster);
        Disaster Update(Disaster updatedDisaster);
        IEnumerable<Disaster> GetActiveList();
        IEnumerable<Disaster> GetList();
        string GetName(int disasterId);
    }
}
