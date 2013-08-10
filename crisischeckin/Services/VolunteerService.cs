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
        private IDataService ourService;

        public VolunteerService(IDataService service = null)
        {
            if (service == null)
            {
                ourService = new DataService();
            }
            else
            {
                ourService = service;
            }
        }

        public Person Register(string firstName, string lastName, string email, string phoneNumber)
        {
            if (string.IsNullOrEmpty(firstName)) { throw new ArgumentNullException("firstName"); }
            if (string.IsNullOrEmpty(lastName)) { throw new ArgumentNullException("lastName"); }
            if (string.IsNullOrEmpty(email)) { throw new ArgumentNullException("email"); }
            if (string.IsNullOrEmpty(phoneNumber)) { throw new ArgumentNullException("phoneNumber"); }

            var foundPerson = ourService.Persons.FirstOrDefault(p => p.Email == email);

            if (foundPerson != null)
            {
                throw new PersonAlreadyExistsException();
            }

            // TODO: eventually support User object
            Person person = new Person() 
            { 
                UserId = null, FirstName = firstName, LastName = lastName, Email = email, PhoneNumber = phoneNumber
            };

            return ourService.AddPerson(person);
        }
    }
}
