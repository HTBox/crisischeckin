using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;

namespace Services
{
    public class VolunteerService : IVolunteer
    {
        public bool Register(string firstName, string lastName, string email, string phoneNumber)
        {
            if (string.IsNullOrEmpty(firstName)) { throw new ArgumentNullException("firstName"); }


            throw new NotImplementedException();
        }
    }
}
