using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Models;
using Services.Interfaces;
using Services.Exceptions;

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

            // check if the start and end date falls within an existing commitment
            // disregard any disasters that are inactive
            Expression<Func<Commitment, bool>> dateInRange = c =>
                (DateTime.Compare(c.StartDate, startDate) <= 0 && DateTime.Compare(c.EndDate, startDate) >= 0) ||
                (DateTime.Compare(c.StartDate, endDate) <= 0 && DateTime.Compare(c.EndDate, endDate) >= 0);

            var hasExistingCommitment = (from c in ourService.Commitments
                                         join d in ourService.Disasters on c.DisasterId equals d.Id
                                         where d.IsActive
                                         select c).Any(dateInRange);

            if (hasExistingCommitment) {
                throw new ArgumentException("there is already a commitment for this date range");
            }
            
            return ourService.AddCommitment(new Commitment()
            {
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

        public Disaster Create(Disaster disaster)
        {
            if (disaster == null) throw new ArgumentNullException("disaster");
            if (String.IsNullOrWhiteSpace(disaster.Name)) throw new ArgumentNullException("disasterName");
            if (ourService.Disasters.Any(d => d.Name == disaster.Name)) throw new DisasterAlreadyExistsException();

            return ourService.AddDisaster(disaster);
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
