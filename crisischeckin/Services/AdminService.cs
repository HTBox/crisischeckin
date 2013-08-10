using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
