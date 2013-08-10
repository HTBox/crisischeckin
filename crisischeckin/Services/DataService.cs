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


        public Person AddPerson(Person newPerson)
        {
            Person result = context.Persons.Add(newPerson);
            context.SaveChanges();
            return result;
        }

        public Commitment AddCommitment(Commitment newCommitment)
        {
            Commitment result = context.Commitments.Add(newCommitment);
            context.SaveChanges();
            return result;
        }

        public void Dispose()
        {
            if (context != null)
                context.Dispose();
        }
    }
}
