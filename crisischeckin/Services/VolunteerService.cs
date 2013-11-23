using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Services.Exceptions;
using Services.Interfaces;
using System.Data.Entity;

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

        public Person Register(string firstName, string lastName, string email, string phoneNumber, int cluster, int userId)
        {
            if (string.IsNullOrWhiteSpace(firstName)) { throw new ArgumentNullException("firstName"); }
            if (string.IsNullOrWhiteSpace(lastName)) { throw new ArgumentNullException("lastName"); }
            if (string.IsNullOrWhiteSpace(email)) { throw new ArgumentNullException("email"); }
            if (string.IsNullOrWhiteSpace(phoneNumber)) { throw new ArgumentNullException("phoneNumber"); }
            if (cluster <= 0) { throw new ArgumentNullException("cluster"); }

            var foundPerson = ourService.Persons.Any(p => p.Email == email);

            if (foundPerson)
            {
                throw new PersonAlreadyExistsException();
            }

            // TODO: eventually support User object
            return ourService.AddPerson(new Person()
            {
                UserId = userId,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber,
                ClusterId = cluster
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
                    bool emailIsInUse = ourService.Persons.Any(p => p.Email == updatedPerson.Email);

                    if (emailIsInUse)
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

            var allCommitments = from c in ourService.Commitments
                         where c.PersonId == person.Id
                         select c;
            var filteredCommitments = allCommitments.Include(c => c.Disaster)
                .Where(c => c.Disaster.IsActive || showInactive);
            return filteredCommitments;
        }

        public IQueryable<Commitment> RetrieveCommitmentsForDisaster(Person person, Disaster disaster)
        {
            if (disaster == null)
                throw new ArgumentNullException("disaster", "Disaster cannot be null");

            return RetrieveCommitments(person, true).Where(c => c.DisasterId == disaster.Id);
        }

        public Person FindByUserId(int userId)
        {
            return ourService.Persons.SingleOrDefault(p => p.UserId == userId);
        }
    }
}
