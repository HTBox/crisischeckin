using System.Linq;
using Models;
using Services.Interfaces;
using Services.Exceptions;
using System;

namespace Services
{
    public class DataService : IDataService
    {
        // This class does not dispose of the context,
        // because the Ninject library takes care of that for us.

        readonly CrisisCheckin context;
        readonly CrisisCheckinMembership membership_context;

        public DataService(CrisisCheckin ctx, CrisisCheckinMembership mctx)
        {
            context = ctx;
            membership_context = mctx;
        }

        public IQueryable<Cluster> Clusters
        {
            get { return context.Clusters; }
        }

        public IQueryable<ClusterCoordinator> ClusterCoordinators
        {
            get { return context.ClusterCoordinators; }
        }

        public IQueryable<Commitment> Commitments
        {
            get { return context.Commitments; }
        }

        public IQueryable<Disaster> Disasters
        {
            get { return context.Disasters; }
        }

        public IQueryable<Person> Persons
        {
            get { return context.Persons; }
        }

        public IQueryable<User> Users
        {
            get { return membership_context.Users; }
        }

        public IQueryable<ClusterCoordinatorLogEntry> ClusterCoordinatorLogEntries
        {
            get { return context.ClusterCoordinatorLogEntries; }
        }

        public Person AddPerson(Person newPerson)
        {
            var result = context.Persons.Add(newPerson);
            context.SaveChanges();
            return result;
        }

        public Person UpdatePerson(Person updatedPerson)
        {
            var result = context.Persons.Find(updatedPerson.Id);

            if (result == null)
                throw new PersonNotFoundException();

            result.FirstName = updatedPerson.FirstName;
            result.LastName = updatedPerson.LastName;
            result.Email = updatedPerson.Email;
            result.PhoneNumber = updatedPerson.PhoneNumber;

            context.SaveChanges();

            return result;
        }

        public void AddCommitment(Commitment newCommitment)
        {
            context.Commitments.Add(newCommitment);
            context.SaveChanges();
        }

        public void RemoveCommitmentById(int id)
        {
            var commitment = context.Commitments.Find(id);
            context.Commitments.Remove(commitment);
            context.SaveChanges();
        }

        public void AddDisaster(Disaster newDisaster)
        {
            context.Disasters.Add(newDisaster);
            context.SaveChanges();
        }

        public Disaster UpdateDisaster(Disaster updatedDisaster)
        {
            var result = context.Disasters.Find(updatedDisaster.Id);

            if (result == null)
                throw new DisasterNotFoundException();

            result.Name = updatedDisaster.Name;
            result.IsActive = updatedDisaster.IsActive;

            context.SaveChanges();

            return result;
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public ClusterCoordinator AddClusterCoordinator(ClusterCoordinator clusterCoordinator)
        {
            context.ClusterCoordinators.Add(clusterCoordinator);
            context.SaveChanges();
            return clusterCoordinator;
        }

        public void RemoveClusterCoordinator(ClusterCoordinator clusterCoordinator)
        {
            // attach the coordinator to delete to the context, if needed. Otherwise the remove/delete won't work
            var coordinatorToDelete = context.ClusterCoordinators.Local.FirstOrDefault(cc => cc.Id == clusterCoordinator.Id);
            if (coordinatorToDelete == null)
                context.ClusterCoordinators.Attach(clusterCoordinator);

            context.ClusterCoordinators.Remove(coordinatorToDelete);
            context.SaveChanges();
        }

        public void AppendClusterCoordinatorLogEntry(ClusterCoordinatorLogEntry clusterCoordinatorLogEntry)
        {
            context.ClusterCoordinatorLogEntries.Add(clusterCoordinatorLogEntry);
            context.SaveChanges();
        }
    }
}