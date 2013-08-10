using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DataService : IDisposable, IDataService
    {
        private readonly CrisisCheckin context;
        public IQueryable<Commitment> Commitments
        { get { return context.Commitments; } }

        public IQueryable<Disaster> Disasters
        { get { return context.Disasters; } }

        public IQueryable<Person> Persons
        { get { return context.Persons; } }

        public IQueryable<User> Users
        { get { return context.Users; } }


        public void Dispose()
        {
            if (context != null)
                context.Dispose();
        }
    }
}
