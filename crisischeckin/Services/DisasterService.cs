using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Services.Interfaces;

namespace Services
{
    public class DisasterService : IDisaster
    {
        private IDataService ourService;

        public DisasterService(IDataService service)
        {
            if (service == null) { throw new ArgumentNullException("service"); }

            ourService = service;
        }

        public Commitment AssignToVolunteer(Disaster disaster, Person person, DateTime startDate, DateTime endDate)
        {
            if (disaster == null) throw new ArgumentNullException("disaster");
            if (person == null) throw new ArgumentNullException("person");
            if (DateTime.Compare(endDate, startDate) < 0) throw new ArgumentException("endDate cannot be earlier than startDate");

            return ourService.AddCommitment(new Commitment() {
                PersonId = person.Id,
                DisasterId = disaster.Id,
                StartDate = startDate,
                EndDate = endDate
            });
        }

        public Disaster Get(int disasterId)
        {
            return ourService.Disasters.SingleOrDefault(d => d.Id.Equals(disasterId));
        }

        public Disaster Create(string disasterName, bool isActive)
        {
           if (String.IsNullOrWhiteSpace(disasterName)) throw new ArgumentNullException("disasterName");

            return ourService.AddDisaster(new Disaster()
                {
                    Name = disasterName,
                    IsActive = isActive
                });
        }

        public void Update(int disasterId, string disasterName, bool isActive)
        {
            Disaster origDisaster = ourService.Disasters.SingleOrDefault(d => d.Id.Equals(disasterId));

            if (origDisaster != null)
            {
                origDisaster.Name = disasterName;
                origDisaster.IsActive = isActive;

                ourService.SubmitChanges();
            }
        }

        public IEnumerable<Disaster> GetActiveList()
        {
            return ourService.Disasters.Where(d => d.IsActive.Equals(true)).OrderBy(d => d.Name);
        }

        public IEnumerable<Disaster> GetList()
        {
            return ourService.Disasters.OrderByDescending(d => d.IsActive).ThenBy(d => d.Name);
        }
    }
}
