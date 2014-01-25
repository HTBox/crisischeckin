using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Services
{
    public class AdminService : IAdmin
    {
        private readonly IDataService dataService;

        public AdminService(IDataService service)
        {
            if (service == null)
                throw new ArgumentNullException("service", "Service Interface must not be null");
            this.dataService = service;
        }

        public IEnumerable<Person> GetVolunteers(Disaster disaster)
        {
            if (disaster == null)
                throw new ArgumentNullException("disaster", "disaster cannot be null");
            var storedDisaster = dataService.Disasters.SingleOrDefault(d => d.Id == disaster.Id);
            if (storedDisaster == null)
                throw new ArgumentException("Disaster was not found", "disaster");
            IQueryable<Person> people = GetPeople(disaster);
            return people;
        }

        public IEnumerable<Person> GetVolunteersForDate(Disaster disaster, DateTime date)
        {
            return GetVolunteersForDate(disaster.Id, date);
        }

        public IEnumerable<Person> GetVolunteersForDate(int disasterId, DateTime date)
        {
            if (0 == disasterId)
                throw new ArgumentException("disasterId must be greater than zero", "disasterId");
            var storedDisaster = dataService.Disasters.SingleOrDefault(x => x.Id.Equals(disasterId));
            if (storedDisaster == null)
                throw new ArgumentException("Disaster was not found", "disaster");

            var commitments = from c in dataService.Commitments
                              where c.DisasterId == disasterId
                              where date >= c.StartDate && date <= c.EndDate
                              select c;

            var people = from c in commitments
                         join p in dataService.Persons on c.PersonId equals p.Id
                         select p;
            people.Include(p => p.Cluster);

            //var p = dataService.Persons.Where(x => x.Id.Equals())

            return people;
        }

        private IQueryable<Person> GetPeople(Disaster disaster)
        {
            var commitments = from c in dataService.Commitments
                              where c.DisasterId == disaster.Id
                              select c;
            var people = from c in commitments
                         join p in dataService.Persons on c.PersonId equals p.Id
                         select p;
            return people;
        }
    }
}
