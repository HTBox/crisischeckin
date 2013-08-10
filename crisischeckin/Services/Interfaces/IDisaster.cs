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
        Commitment AssignToVolunteer(Disaster disaster, Person person, DateTime startDate, DateTime endDate);
        Disaster Create(string disasterName, bool IsActive);
        void Update(int disasterId, string disasterName, bool isActive);
        IEnumerable<Disaster> GetActiveList();
        IEnumerable<Disaster> GetList();
    }
}
