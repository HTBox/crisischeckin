using System;
using System.Linq;
using Models;

namespace Services.Interfaces
{
    public interface IVolunteerService
    {
        Person Register(string firstName, string lastName, string email, string phoneNumber, int cluster, int userId);

        Person UpdateDetails(Person person);

        IQueryable<Commitment> RetrieveCommitments(int personId, bool showInactive);

        IQueryable<Commitment> RetrieveCommitmentsForDisaster(Person person, Disaster disaster);

        Person FindByUserId(int userId);
        Person GetPersonDetailsForChangeContactInfo(int userId);

		bool UsernameAvailable(string userName);
        bool EmailAlreadyInUse(string email);
        User FindUserByEmail(string email);
    }
}
