using Models;
using System.Linq;

namespace Services.Interfaces
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
        IQueryable<Cluster> Clusters { get; }
        IQueryable<VolunteerType> VolunteerTypes { get; }
        IQueryable<DisasterCluster> DisasterClusters { get; }
        IQueryable<ClusterCoordinator> ClusterCoordinators { get; }
        IQueryable<ClusterCoordinatorLogEntry> ClusterCoordinatorLogEntries { get; }
        Person AddPerson(Person newPerson);
        Person UpdatePerson(Person updatedPerson);
        void AddCommitment(Commitment newCommitment);
        void RemoveCommitmentById(int id);
        void AddDisaster(Disaster newDisaster);
        Disaster UpdateDisaster(Disaster updatedDisaster);
        void AddDisasterCluster(DisasterCluster newDisasterCluster);
        void RemoveDisasterCluster(DisasterCluster newDisasterCluster);
        void SubmitChanges();
        ClusterCoordinator AddClusterCoordinator(ClusterCoordinator clusterCoordinator);
        void AppendClusterCoordinatorLogEntry(ClusterCoordinatorLogEntry clusterCoordinatorLogEntry);
        void RemoveClusterCoordinator(ClusterCoordinator clusterCoordinator);
        Commitment UpdateCommitment(Commitment updatedCommitment);
    }
}