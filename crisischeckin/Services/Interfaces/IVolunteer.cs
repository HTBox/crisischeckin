using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IVolunteer
    {
        bool Register(string firstName, string lastName, string email, string phoneNumber);


    }
}
