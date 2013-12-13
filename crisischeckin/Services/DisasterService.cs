using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Models;
using Services.Interfaces;
using Services.Exceptions;

namespace Services
{
    public class DisasterService : IDisaster
    {
        private readonly IDataService _dataService;

        public DisasterService(IDataService service)
        {
            if (service == null) { throw new ArgumentNullException("service"); }

            _dataService = service;
        }

        public void AssignToVolunteer(Disaster disaster, Person person, DateTime startDate, DateTime endDate)
        {
            if (disaster == null) throw new ArgumentNullException("disaster");
            if (person == null) throw new ArgumentNullException("person");
            if (DateTime.Compare(endDate, startDate) < 0) throw new ArgumentException("endDate cannot be earlier than startDate");

            // check if the start and end date falls within an existing commitment
            // disregard any disasters that are inactive
            Expression<Func<Commitment, bool>> dateInRange = c =>
                (DateTime.Compare(c.StartDate, startDate) <= 0 && DateTime.Compare(c.EndDate, startDate) >= 0) ||
                (DateTime.Compare(c.StartDate, endDate) <= 0 && DateTime.Compare(c.EndDate, endDate) >= 0);

            var hasExistingCommitment = (from c in _dataService.Commitments
                                         join d in _dataService.Disasters on c.DisasterId equals d.Id
                                         where d.IsActive
                                         select c).Any(dateInRange);

            if (hasExistingCommitment) {
                throw new ArgumentException("there is already a commitment for this date range");
            }
            
            _dataService.AddCommitment(new Commitment()
                                       {
                                           PersonId = person.Id,
                                           DisasterId = disaster.Id,
                                           StartDate = startDate,
                                           EndDate = endDate
                                       });
        }

        public Disaster Get(int disasterId)
        {
            return _dataService.Disasters.SingleOrDefault(d => d.Id.Equals(disasterId));
        }

        public Disaster Create(Disaster disaster)
        {
            if (disaster == null) throw new ArgumentNullException("disaster");
            if (String.IsNullOrWhiteSpace(disaster.Name)) throw new ArgumentNullException("disasterName");
            if (_dataService.Disasters.Any(d => d.Name == disaster.Name)) throw new DisasterAlreadyExistsException();

            return _dataService.AddDisaster(disaster);
        }

        public void Update(int disasterId, string disasterName, bool isActive)
        {
            Disaster origDisaster = _dataService.Disasters.SingleOrDefault(d => d.Id.Equals(disasterId));

            if (origDisaster != null)
            {
                origDisaster.Name = disasterName;
                origDisaster.IsActive = isActive;

                _dataService.SubmitChanges();
            }
        }

        public IEnumerable<Disaster> GetActiveList()
        {
            return _dataService.Disasters.Where(d => d.IsActive.Equals(true)).OrderBy(d => d.Name);
        }

        public IEnumerable<Disaster> GetList()
        {
            return _dataService.Disasters.OrderByDescending(d => d.IsActive).ThenBy(d => d.Name);
        }
    }
}
