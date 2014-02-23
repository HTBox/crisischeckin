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
            if (disaster == null)
                throw new ArgumentNullException("disaster", "disaster cannot be null");

            var people = from p in dataService.Persons
                          join c in dataService.Commitments on p.Id equals c.PersonId
                          where c.DisasterId == disaster.Id
                          where date >= c.StartDate && date <= c.EndDate
                          select p;
            people.Include(x => x.Cluster);

            return people;
        }

        private IQueryable<Person> GetPeople(Disaster disaster)
        {
            var people = from p in dataService.Persons
                          join c in dataService.Commitments on p.Id equals c.PersonId
                          where c.DisasterId == disaster.Id
                          select p;

            return people;
        }
    }
}
