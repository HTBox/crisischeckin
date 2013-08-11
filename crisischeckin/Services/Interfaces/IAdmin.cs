using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAdmin
    {
        IEnumerable<Person> GetVolunteers(Disaster disaster);
        IEnumerable<Person> GetVolunteersForDate(Disaster disaster, DateTime date);
    }
}
