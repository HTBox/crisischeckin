using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;

namespace Services
{
    public class DisasterService : IDisaster
    {
        public bool AssignToVolunteer(int disasterId, int volunteerId, DateTime startDate, DateTime endDate)
        {
            if (DateTime.Compare(endDate, startDate) < 0) throw new ArgumentException("endDate cannot be earlier than startDate");


            // TODO: use entity framework...

            return true;
        }


        public void Create(string disasterName, bool IsActive)
        {
            throw new NotImplementedException();
        }

        public List<Models.Disaster> GetActiveList()
        {
            throw new NotImplementedException();
        }

        public List<Models.Disaster> GetList()
        {
            throw new NotImplementedException();
        }
    }
}
