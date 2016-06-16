using Models;
using System.Linq;
<<<<<<< HEAD
using System.Threading.Tasks;
=======
>>>>>>> refs/remotes/HTBox/mobile_rework

namespace Services.Interfaces
{
    // This service manages the communication to the Models project
    // using the live database in production, but it can be mocked
    // for test purposes.
    public interface IDataService
    {
        IQueryable<Commitment> Commitments { get; }
        IQueryable<Disaster> Disasters { get; }
        IQueryable<Request> Requests { get; }
        IQueryable<Person> Persons { get; }
        IQueryable<User> Users { get; }
        IQueryable<ClusterGroup> ClusterGroups { get; }
        IQueryable<Cluster> Clusters { get; }
        IQueryable<VolunteerType> VolunteerTypes { get; }
        IQueryable<DisasterCluster> DisasterClusters { get; }
        IQueryable<ClusterCoordinator> ClusterCoordinators { get; }
        IQueryable<ClusterCoordinatorLogEntry> ClusterCoordinatorLogEntries { get; }
        IQueryable<Organization> Organizations { get; }
        IQueryable<Resource> Resources { get; }
        IQueryable<Contact> Contacts { get; }
        IQueryable<ResourceType> ResourceTypes { get; }

        Organization AddOrganization(Organization newOrganization);

        void VerifyOrganization(int organizationId);
        Person AddPerson(Person newPerson);
        Person UpdatePerson(Person updatedPerson);
        void AddResource(Resource newResource);
        void AddContact(Contact newContact);
        void AddCommitment(Commitment newCommitment);
        void AddCluster(Cluster newCluster);
        void RemoveCluster(Cluster clusterToDelete);
<<<<<<< HEAD
        Task AssignRequestToUserAsync(int userId, int requestId);
        Task CompleteRequestAsync(int requestId);
=======
>>>>>>> refs/remotes/HTBox/mobile_rework
        Cluster UpdateCluster(Cluster updatedCluster);
        void AddClusterGroup(ClusterGroup newCluster);
        void RemoveClusterGroup(ClusterGroup clusterToDelete);
        ClusterGroup UpdateClusterGroup(ClusterGroup updatedCluster);
        void RemoveCommitmentById(int id);
        void RemoveResourceById(int id);
        void AddDisaster(Disaster newDisaster);
        Disaster UpdateDisaster(Disaster updatedDisaster);
        void AddDisasterCluster(DisasterCluster newDisasterCluster);
        void RemoveDisasterCluster(DisasterCluster newDisasterCluster);
        void SubmitChanges();
        ClusterCoordinator AddClusterCoordinator(ClusterCoordinator clusterCoordinator);
        void AppendClusterCoordinatorLogEntry(ClusterCoordinatorLogEntry clusterCoordinatorLogEntry);
        void RemoveClusterCoordinator(ClusterCoordinator clusterCoordinator);
        Commitment UpdateCommitment(Commitment updatedCommitment);
        void RemoveClusterCoordinator(int personId, int clusterId, int disasterId);
    }
}