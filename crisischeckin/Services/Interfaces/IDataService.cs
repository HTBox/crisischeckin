using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    // This service manages the communication to the Models project
    // using the live database in production, but it can be mocked
    // for test purposes.
    public interface IDataService
    {
        IQueryable<Commitment> Commitments { get; }
        IQueryable<Disaster> Disasters { get; }
        IQueryable<Person> Persons { get; }
        IQueryable<User> Users { get; }
    }
}
