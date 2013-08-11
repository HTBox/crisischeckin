using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Services.Exceptions;
using Services.Interfaces;

namespace Services
{
    public class VolunteerService : IVolunteer
    {
        private readonly IDataService ourService;

        public VolunteerService(IDataService service)
        {
            if (service == null) { throw new ArgumentNullException("service"); }

            ourService = service;
        }

        public Person Register(string firstName, string lastName, string email, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(firstName)) { throw new ArgumentNullException("firstName"); }
            if (string.IsNullOrWhiteSpace(lastName)) { throw new ArgumentNullException("lastName"); }
            if (string.IsNullOrWhiteSpace(email)) { throw new ArgumentNullException("email"); }
            if (string.IsNullOrWhiteSpace(phoneNumber)) { throw new ArgumentNullException("phoneNumber"); }

            var foundPerson = ourService.Persons.Any(p => p.Email == email);

            if (foundPerson)
            {
                throw new PersonAlreadyExistsException();
            }

            // TODO: eventually support User object
            return ourService.AddPerson(new Person()
            {
                UserId = null,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber
            });
        }

        public Person UpdateDetails(Person updatedPerson)
        {
            if (updatedPerson == null) throw new ArgumentNullException("updatedPerson");

            var foundPerson = ourService.Persons.SingleOrDefault(p => p.Id == updatedPerson.Id);

            if (foundPerson != null)
            {
                if (foundPerson.Email != updatedPerson.Email)
                {
                    // check that new email isn't already in use
                    var u = ourService.Persons.Any(p => p.Email == updatedPerson.Email);

                    if (u != null)
                    {
                        throw new PersonEmailAlreadyInUseException();
                    }
                }

                return ourService.UpdatePerson(updatedPerson);
            }
            else
            {
                throw new PersonNotFoundException();
            }
        }

        public IQueryable<Commitment> RetrieveCommitments(Person person, bool showInactive)
        {
            if (person == null)
                throw new ArgumentNullException("person", "Person cannot be null");

            var answer = from c in ourService.Commitments
                         where c.PersonId == person.Id
                         where showInactive || c.Disaster.IsActive
                         select c;
            return answer;
        }

        public IQueryable<Commitment> RetrieveCommitmentsForDisaster(Person person, Disaster disaster)
        {
            if (disaster == null)
                throw new ArgumentNullException("disaster", "Disaster cannot be null");

            return RetrieveCommitments(person, true).Where(c => c.DisasterId == disaster.Id);
        }

    }
}
