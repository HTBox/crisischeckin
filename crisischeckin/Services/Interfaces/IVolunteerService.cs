using System;
using System.Linq;
using System.Collections.Generic;
using Models;

namespace Services.Interfaces
{
    public interface IVolunteerService
    {
        Person Register(string firstName, string lastName, int? OrganizationId, string email, string phoneNumber, int userId);

        IEnumerable<Person> GetList();

        Person UpdateDetails(Person person);

        IQueryable<Commitment> RetrieveCommitments(int personId, bool showInactive);

        IQueryable<Commitment> RetrieveCommitmentsForDisaster(Person person, Disaster disaster);

        IEnumerable<Commitment> GetCommitmentsForOrganization(int organizationId, bool includeInactive);

        Person FindByUserId(int userId);
        Person GetPersonDetailsForChangeContactInfo(int userId);

		bool UsernameAvailable(string userName);
        bool EmailAlreadyInUse(string email);
        User FindUserByEmail(string email);

        void UpdateCommitment(Commitment commitment);

        IEnumerable<Person> GetVolunteersByOrganization(int organizationId);
        void AddVolunteerToOrganization(int organizationId, int personId);
        void RemoveVolunteerFromOrganization(int organizationId, int personId);
        void PromoteVolunteerToOrganizationAdmin(int organizationId, int personId);
        void DemoteVolunteerFromOrganizationAdmin(int organizationId, int personId);
    }
}
