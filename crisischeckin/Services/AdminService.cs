using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Services.Interfaces;
using System.Data.Entity;

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

        public IEnumerable<Person> GetVolunteersForDate(int disasterId, DateTime date, bool clusterCoordinatorsOnly, bool checkedInOnly = false, IEnumerable<int> inClusterIds = null)
        {
            if (disasterId <= 0)
                throw new ArgumentException("disasterId is invalid.", "disasterId");

            var people = clusterCoordinatorsOnly
                ? GetClusterCoordinatorsForDateQueryable(disasterId, date, checkedInOnly, inClusterIds)
                : GetVolunteersForDateQueryable(disasterId, date, checkedInOnly, inClusterIds);

            if (people == null)
                throw new NullReferenceException(string.Format("Attempt to get volunteers for disaster ID {0} returned null.", disasterId));
            return people.ToList();
        }

        private IQueryable<Person> GetClusterCoordinatorsForDateQueryable(int disasterId, DateTime date, bool checkedInOnly, IEnumerable<int> inClusterIds)
        {
            if (disasterId <= 0)
                throw new ArgumentException("disasterId must be greater than zero", "disasterId");

            var inClusterIdsArray = inClusterIds?.ToArray() ?? new int[0];
            var hasClusters = inClusterIdsArray.Length == 0;

            var people = from cc in _dataService.ClusterCoordinators
                         where cc.DisasterId == disasterId
                         join c in Commitment.FilteredByStatus(_dataService.Commitments, checkedInOnly)
                            on cc.PersonId equals c.PersonId
                         where c.DisasterId == disasterId
                         where date >= c.StartDate && date <= c.EndDate
                         where hasClusters || inClusterIdsArray.Any(cid => cid == c.ClusterId)
                         select cc.Person;

            return people.Distinct();
        }

        private IQueryable<Person> GetVolunteersForDateQueryable(int disasterId, DateTime date, bool checkedInOnly, IEnumerable<int> inClusterIds)
        {
            if (disasterId <= 0)
                throw new ArgumentException("disasterId must be greater than zero", "disasterId");

            var inClusterIdsArray = inClusterIds?.ToArray() ?? new int[0];
            var hasClusters = inClusterIdsArray.Length == 0;

            var people = from p in _dataService.Persons
                         join c in Commitment.FilteredByStatus(_dataService.Commitments, checkedInOnly)
                            on p.Id equals c.PersonId
                         where c.DisasterId == disasterId
                         where date >= c.StartDate && date <= c.EndDate
                         where hasClusters || inClusterIdsArray.Any(cid => cid == c.ClusterId)
                         select p;

            return people.Distinct();
        }

        public IEnumerable<Contact> GetContactsForDisaster(int disasterId)
        {
            IEnumerable<Contact> contacts;
            contacts = _dataService.Contacts.Include("Organization").Include("Person");

            //need to filter by current disaster after reworking contacts database

            if (contacts == null)
                throw new NullReferenceException(string.Format("Attempt to get volunteers for disaster ID {0} returned null.", disasterId));
            return contacts.ToList();
        }

        public IEnumerable<Resource> GetResourceCheckinsForOrganization(int organizationId)
        {
            IEnumerable<Resource> resources;
            resources = GetResourcesForOrganization(organizationId);

            if (resources == null)
                throw new NullReferenceException(string.Format("Attempt to get volunteers for organization ID {0} returned null.", organizationId));
            return resources.ToList();
        }

        public IEnumerable<Resource> GetResourceCheckinsForDisaster(int disasterId, DateTime? commitmentDate)
        {
            IEnumerable<Resource> resources;
            resources = GetResources(disasterId, commitmentDate);

            if (resources == null)
                throw new NullReferenceException(string.Format("Attempt to get volunteers for disaster ID {0} returned null.", disasterId));
            return resources.ToList();
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

        private IQueryable<Resource> GetResourcesForOrganization(int organizationId)
        {
            if (organizationId <= 0)
                throw new ArgumentException("organizationId is invalid.", "organizationId");

            var results = from r in _dataService.Resources
                          where r.Allocator.OrganizationId == organizationId
                          select r;

            return results;
        }

        private IQueryable<Resource> GetResources(int disasterId, DateTime? date)
        {
            if (disasterId <= 0)
                throw new ArgumentException("disasterId is invalid.", "disasterId");

            var results = from r in _dataService.Resources
                          where r.DisasterId == disasterId
                          select r;

            if (date.HasValue)
            {
                results = results.Where(r => date >= r.StartOfAvailability && date <= r.EndOfAvailability);
            }

            return results;
        }

        private IQueryable<Person> GetPeople(int disasterId, bool checkedInOnly)
        {
            if (disasterId <= 0)
                throw new ArgumentException("disasterId is invalid.", "disasterId");

            var people = from p in _dataService.Persons
                         join c in Commitment.FilteredByStatus(_dataService.Commitments, checkedInOnly)
                            on p.Id equals c.PersonId
                         where c.DisasterId == disasterId
                         select p;

            return people.Distinct();
        }
    }
}
