using Models;
using Services.Exceptions;
using Services.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;

namespace Services
{
    public class VolunteerService : IVolunteerService
    {
        private readonly IDataService ourService;

        public VolunteerService(IDataService service)
        {
            if (service == null) { throw new ArgumentNullException("service"); }

            ourService = service;
        }

        public Person Register(string firstName, string lastName, int? OrganizationId, string email, string phoneNumber, int userId)
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
            return ourService.AddPerson(new Person
            {
                UserId = userId,
                FirstName = firstName,
                LastName = lastName,
                OrganizationId = OrganizationId,
                Email = email,
                PhoneNumber = phoneNumber
            });
        }

        public Person UpdateDetails(Person updatedPerson)
        {
            if (updatedPerson == null)
                throw new ArgumentNullException("updatedPerson");

            var foundPerson = ourService.Persons.SingleOrDefault(p => p.UserId == updatedPerson.UserId);

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
                    foundPerson.Email = updatedPerson.Email;
                }
                
                // update the found person with any appropriate changes
                if (!string.IsNullOrEmpty(updatedPerson.FirstName))
                {
                    foundPerson.FirstName = updatedPerson.FirstName;
                }

                if (!string.IsNullOrEmpty(updatedPerson.LastName))
                {
                    foundPerson.LastName = updatedPerson.LastName;
                }

                foundPerson.PhoneNumber = updatedPerson.PhoneNumber;

                foundPerson.OrganizationId = updatedPerson.OrganizationId;

                return ourService.UpdatePerson(foundPerson);
            }
            throw new PersonNotFoundException();
        }

        public IQueryable<Commitment> RetrieveCommitments(int personId, bool showInactive)
        {
            var filteredCommitments = from c in ourService.Commitments.Include(c => c.Disaster)
                                      where c.PersonId == personId &&
                                      (c.Disaster.IsActive || showInactive)
                                      orderby c.StartDate
                                      select c;

            return filteredCommitments;
        }

        public IQueryable<Commitment> RetrieveCommitmentsForDisaster(Person person, Disaster disaster)
        {
            if (disaster == null)
                throw new ArgumentNullException("disaster", "Disaster cannot be null");

            return RetrieveCommitments(person.Id, true).Where(c => c.DisasterId == disaster.Id);
        }

        public IEnumerable<Commitment> GetCommitmentsForOrganization(int organizationId, bool includeInactive)
        {
            var result = from commitment in ourService.Commitments
                         where commitment.Person.OrganizationId == organizationId
                         && (commitment.Disaster.IsActive || includeInactive)
                         select commitment;

            return result;
        }

        public void UpdateCommitment(Commitment commitment)
        {
            if (commitment == null)
                throw new ArgumentNullException("commitment", "Commitment cannot be null");

            ourService.UpdateCommitment(commitment);
        }

        public Person FindByUserId(int userId)
        {
            return ourService.Persons.Include(p => p.Organization).SingleOrDefault(p => p.UserId == userId);
        }

        public Person GetPersonDetailsForChangeContactInfo(int userId)
        {
            var result = ourService.Persons.Where(p => p.UserId == userId).Select(per => new
            {
                Email = per.Email,
                PhoneNumber = per.PhoneNumber,
                OrganizationId = per.OrganizationId
            }).FirstOrDefault();

            if (result == null)
                throw new PersonNotFoundException();

            return new Person
            {
                Email = result.Email,
                PhoneNumber = result.PhoneNumber,
                OrganizationId = result.OrganizationId
            };
        }

        public bool UsernameAvailable(string userName)
        {
            return ourService.Users.Count(p => p.UserName == userName) <= 0;
        }

        public bool EmailAlreadyInUse(string email)
        {
            if (ourService.Persons.Any(p => p.Email == email))
                return true;
            return false;
        }

        public User FindUserByEmail(string email)
        {
            var userId = ourService.Persons
                .Where(p => String.Compare(p.Email, email, StringComparison.OrdinalIgnoreCase) == 0)
                .Select(p => p.UserId)
                .FirstOrDefault();

            return userId.HasValue ? ourService.Users.FirstOrDefault(u => u.Id == userId) : null;
        }

        public IEnumerable<Person> GetVolunteersByOrganization(int organizationId)
        {
            var result = ourService.Persons.Where(person => person.OrganizationId == organizationId);
            return result.ToList();
        }

        public void PromoteVolunteerToOrganizationAdmin(int organizationId, int personId)
        {
            ourService.PromoteVolunteerToOrganizationAdmin(organizationId, personId);
        }

        public void DemoteVolunteerFromOrganizationAdmin(int organizationId, int personId)
        {
            ourService.DemoteVolunteerFromOrganizationAdmin(organizationId, personId);
        }
    }
}
