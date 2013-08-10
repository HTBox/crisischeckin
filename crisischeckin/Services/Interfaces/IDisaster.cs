using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IDisaster
    {
        bool AssignToVolunteer(int disasterId, int volunteerId, DateTime startDate, DateTime endDate);
        void Create(string disasterName, bool IsActive);
        List<Models.Disaster> GetActiveList();
        List<Models.Disaster> GetList();
    }
}
