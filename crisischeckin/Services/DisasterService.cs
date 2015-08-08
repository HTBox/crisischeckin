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

        public void AssignToVolunteer(int disasterId, int personId, DateTime startDate, DateTime endDate,
            int volunteerType, int clusterId)
        {
            if (DateTime.Compare(endDate, startDate) < 0)
            {
                throw new ArgumentException("Please enter a end date that is greater than or equal to the start date.");
            }
            if (DateTime.Compare(DateTime.Today, startDate) > 0)
            {
                throw new ArgumentException("Please enter a start date that is greater than or equal to today's date.");
            }

            // check if the start and end date falls within an existing commitment
            // disregard any disasters that are inactive
            Expression<Func<Commitment, bool>> dateInRange = c =>
                (DateTime.Compare(c.StartDate, endDate) <= 0) &&
                (DateTime.Compare(c.EndDate, startDate) >= 0);

            var hasExistingCommitment = (from c in _dataService.Commitments
                join d in _dataService.Disasters on c.DisasterId equals d.Id 
                where d.IsActive && c.PersonId == personId
                select c).Any(dateInRange);

            if (hasExistingCommitment)
            {
                throw new ArgumentException("there is already a commitment for this date range");
            }

            _dataService.AddCommitment(new Commitment
            {
                PersonId = personId,
                DisasterId = disasterId,
                StartDate = startDate,
                EndDate = endDate,
                VolunteerTypeId = volunteerType,
                ClusterId = clusterId
            });
        }

        public Disaster Get(int disasterId)
        {
            return _dataService.Disasters.SingleOrDefault(d => d.Id.Equals(disasterId));
        }

        public string GetName(int disasterId)
        {
            return _dataService.Disasters.Where(d => d.Id.Equals(disasterId)).Select(d => d.Name).FirstOrDefault();
        }

        public void Create(Disaster disaster)
        {
            if (disaster == null) throw new ArgumentNullException("disaster");
            if (String.IsNullOrWhiteSpace(disaster.Name)) throw new ArgumentNullException("disaster");
            if (_dataService.Disasters.Any(d => d.Name == disaster.Name)) throw new DisasterAlreadyExistsException();
            // Why should disaster name be unique?

            _dataService.AddDisaster(disaster);
        }

        public void RemoveCommitmentById(int commitmentId)
        {
            //  idempotent method - don't care if the id does not exist. 
            _dataService.RemoveCommitmentById(commitmentId);
        }

        public Disaster Update(Disaster updatedDisaster)
        {

            if (_dataService.Disasters.Count(d => d.Id == updatedDisaster.Id) == 0)
                throw new DisasterNotFoundException();

            if (_dataService.Disasters.Any(d => d.Name == updatedDisaster.Name && d.Id != updatedDisaster.Id)) 
                throw new DisasterAlreadyExistsException();

            var result = _dataService.UpdateDisaster(updatedDisaster);

            return result;
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
