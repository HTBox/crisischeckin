using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Services.Interfaces;

namespace Services
{
    public class AdminService : IAdmin
    {
        private readonly IDataService _dataService;

        public AdminService(IDataService service)
        {
            if (service == null)
                throw new ArgumentNullException("service", "Service Interface must not be null");
            _dataService = service;
        }

        public IEnumerable<Person> GetVolunteers(Disaster disaster, bool checkedInOnly = false)
        {
            if (disaster == null)
                throw new ArgumentNullException("disaster", "disaster cannot be null");

            var people = GetPeople(disaster.Id, checkedInOnly);

            if (people == null)
                throw new NullReferenceException(string.Format("Attempt to get volunteers for disaster ID {0} returned null.", disaster.Id));
            return people.ToList();
        }

        public IEnumerable<Person> GetVolunteersForDate(Disaster disaster, DateTime date, bool clusterCoordinatorsOnly, bool checkedInOnly = false)
        {
            if (disaster == null)
                throw new ArgumentNullException("disaster", "disaster cannot be null");

            return GetVolunteersForDate(disaster.Id, date, clusterCoordinatorsOnly, checkedInOnly);
        }

        public IEnumerable<Person> GetVolunteersForDate(int disasterId, DateTime date, bool clusterCoordinatorsOnly, bool checkedInOnly = false)
        {
            if (disasterId <= 0)
                throw new ArgumentException("disasterId is invalid.", "disasterId");

            var people = clusterCoordinatorsOnly
                ? GetClusterCoordinatorsForDateQueryable(disasterId, date, checkedInOnly)
                : GetVolunteersForDateQueryable(disasterId, date, checkedInOnly);

            if (people == null)
                throw new NullReferenceException(string.Format("Attempt to get volunteers for disaster ID {0} returned null.", disasterId));
            return people.ToList();
        }

        private IQueryable<Person> GetClusterCoordinatorsForDateQueryable(int disasterId, DateTime date, bool checkedInOnly)
        {
            if (disasterId <= 0)
                throw new ArgumentException("disasterId must be greater than zero", "disasterId");

            var people = from cc in _dataService.ClusterCoordinators
                         where cc.DisasterId == disasterId
                         join c in _dataService.Commitments on cc.PersonId equals c.PersonId
                         where c.DisasterId == disasterId
                         where !checkedInOnly || c.PersonIsCheckedIn
                         where date >= c.StartDate && date <= c.EndDate
                         select cc.Person;

            return people.Distinct();
        }

        private IQueryable<Person> GetVolunteersForDateQueryable(int disasterId, DateTime date, bool checkedInOnly)
        {
            if (disasterId <= 0)
                throw new ArgumentException("disasterId must be greater than zero", "disasterId");

            var people = from p in _dataService.Persons
                         join c in _dataService.Commitments on p.Id equals c.PersonId
                         where c.DisasterId == disasterId
                         where !checkedInOnly || c.PersonIsCheckedIn
                         where date >= c.StartDate && date <= c.EndDate
                         select p;
            //people.Include(x => x.Cluster);

            return people.Distinct();
        }

        public IEnumerable<Person> GetVolunteersForDisaster(int disasterId, DateTime? commitmentDate, bool checkedInOnly = false)
        {
            IEnumerable<Person> people;
            if (commitmentDate.HasValue)
            {
                people = GetVolunteersForDate(disasterId, commitmentDate.Value, clusterCoordinatorsOnly: false, checkedInOnly: checkedInOnly);
            }
            else
            {
                people = GetPeople(disasterId, checkedInOnly);
            }

            //remove commitments not pertaining to this disaseter

            if (people == null)
                throw new NullReferenceException(string.Format("Attempt to get volunteers for disaster ID {0} returned null.", disasterId));
            return people.ToList();
        }

        private IQueryable<Person> GetPeople(int disasterId, bool checkedInOnly)
        {
            if (disasterId <= 0)
                throw new ArgumentException("disasterId is invalid.", "disasterId");

            var people = from p in _dataService.Persons
                         join c in _dataService.Commitments on p.Id equals c.PersonId
                         where c.DisasterId == disasterId
                         where !checkedInOnly || c.PersonIsCheckedIn
                         select p;

            return people.Distinct();
        }
    }
}
