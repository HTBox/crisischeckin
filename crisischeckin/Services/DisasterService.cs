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
    }
}
