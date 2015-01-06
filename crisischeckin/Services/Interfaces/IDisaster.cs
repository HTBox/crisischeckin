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
        void AssignToVolunteer(int disasterId, int personId, DateTime startDate, DateTime endDate, int volunteerType);
        void RemoveCommitmentById(int commitmentId);
        Disaster Get(int disasterId);
        void Create(Disaster disaster);
        void Update(int disasterId, string disasterName, bool isActive);
        IEnumerable<Disaster> GetActiveList();
        IEnumerable<Disaster> GetList();
        string GetName(int disasterId);
    }
}
