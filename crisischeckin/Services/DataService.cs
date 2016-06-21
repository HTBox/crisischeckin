using Models;
using Services.Exceptions;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Models.Migrations;

namespace Services
{
    public class DataService : IDataService
    {
        // This class does not dispose of the context, because the Ninject library takes care of
        // that for us.

        private readonly CrisisCheckin context;
        private readonly CrisisCheckinMembership membership_context;

        public DataService(CrisisCheckin ctx, CrisisCheckinMembership mctx)
        {
            context = ctx;
            membership_context = mctx;
        }

        public IQueryable<ClusterGroup> ClusterGroups
        {
            get { return context.ClusterGroups; }
        }

        public IQueryable<Cluster> Clusters
        {
            get { return context.Clusters; }
        }

        public IQueryable<VolunteerType> VolunteerTypes
        {
            get { return context.VolunteerTypes; }
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

        public IQueryable<Request> Requests
        {
            get { return context.Requests; }
        }

        public IQueryable<DisasterCluster> DisasterClusters
        {
            get { return context.DisasterClusters; }
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

        public IQueryable<Organization> Organizations
        {
            get { return context.Organizations; }
        }

        public IQueryable<Resource> Resources
        {
            get { return context.Resources; }
        }

        public IQueryable<Contact> Contacts
        {
            get { return context.Contacts; }
        }

        public IQueryable<ResourceType> ResourceTypes
        {
            get { return context.ResourceTypes; }
        }

        public Organization AddOrganization(Organization newOrganization)
        {
            var result = context.Organizations.Add(newOrganization);
            context.SaveChanges();
            return result;
        }

        public void VerifyOrganization(int organizationId)
        {
            var organization = context.Organizations.FirstOrDefault(x => x.OrganizationId == organizationId);
            if (organization != null)
            {
                organization.Verified = true;
                context.SaveChanges();
            }
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
            result.OrganizationId = updatedPerson.OrganizationId;

            context.SaveChanges();

            return result;
        }



        public void AddContact(Contact newContact)
        {
            context.Contacts.Add(newContact);
            context.SaveChanges();
        }

        public async Task AddResourceAsync(Resource newResource)
        {
            context.Resources.Add(newResource);
            await context.SaveChangesAsync();
        }

        public async Task<Resource> FindResourceByIdAsync(int? id)
        {
            var resource = await context.Resources.Where(r => r.ResourceId == id)
                                    .Include(r => r.Person)
                                    .Include(r => r.ResourceType)
                                    .FirstOrDefaultAsync();

            return resource;
        }

        public async Task<Resource> UpdateResourceAsync(Resource updatedResource)
        {
            var result = await context.Resources.FindAsync(updatedResource.ResourceId);

            if (result == null)
                throw new DisasterNotFoundException();

            result.Allocator = updatedResource.Allocator == null ? null : Organizations.First(org => org.OrganizationId == updatedResource.Allocator.OrganizationId);
            result.Description = updatedResource.Description;
            result.DisasterId = updatedResource.DisasterId;
            result.StartOfAvailability = updatedResource.StartOfAvailability;
            result.EndOfAvailability = updatedResource.EndOfAvailability;
            result.EntryMade = updatedResource.EntryMade;
            result.Location = updatedResource.Location;
            result.PersonId = updatedResource.PersonId;
            result.Qty = updatedResource.Qty;
            result.ResourceTypeId = updatedResource.ResourceTypeId;
            result.Status = updatedResource.Status;

            context.SaveChanges();

            return result;
        }

        public async Task RemoveResourceByIdAsync(int id)
        {
            var resource = await context.Resources.FindAsync(id);

            if (resource == null)
            {
                throw new ResourceNotFoundException();
            }

            context.Resources.Remove(resource);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Resource>> GetAllResourcesAsync()
        {
            return await context.Resources.Include(r => r.Disaster)
                                               .Include(r => r.Allocator)
                                               .Include(r => r.Person)
                                               .Include(r => r.ResourceType)
                                               .ToListAsync();
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

        public Commitment UpdateCommitment(Commitment updatedCommitment)
        {
            var result = context.Commitments.Find(updatedCommitment.Id);

            if (result == null)
                throw new CommitmentNotFoundException();

            result.StartDate = updatedCommitment.StartDate;
            result.EndDate = updatedCommitment.EndDate;
            result.Status = updatedCommitment.Status;
            result.VolunteerType = updatedCommitment.VolunteerType;
            result.ClusterId = updatedCommitment.ClusterId;

            context.SaveChanges();

            return result;
        }

        public void RemoveClusterCoordinator(int personId, int clusterId, int disasterId)
        {
            var clusterCoordinator = context.ClusterCoordinators.SingleOrDefault(
                x => x.PersonId == personId && x.ClusterId == clusterId && x.DisasterId == disasterId);
            if (clusterCoordinator == null) return;
            context.ClusterCoordinators.Remove(clusterCoordinator);
            context.SaveChanges();
        }

        public void AddDisaster(Disaster newDisaster)
        {
            context.Disasters.Add(newDisaster);
            context.SaveChanges();
        }

        public void AddCluster(Cluster newCluster)
        {
            context.Clusters.Add(newCluster);
            context.SaveChanges();
        }

        public void RemoveCluster(Cluster cluster)
        {
            // attach the cluster to delete to the context, if needed. Otherwise the remove/delete won't work
            var cluster2Del = context.Clusters.Local.FirstOrDefault(cc => cc.Id == cluster.Id);
            if (cluster2Del == null)
                context.Clusters.Attach(cluster);

            context.Clusters.Remove(cluster);
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

        public async Task AssignRequestToUserAsync(int userId, int requestId)
        {
            var result = await context.Requests.FindAsync(requestId);

            if (result == null)
                throw new ArgumentException("The specified request does not exist.");

            if (result.AssigneeId.HasValue)
                throw new InvalidOperationException("Cannot assign a request that has already been assigned.");

            result.AssigneeId = userId;

            await context.SaveChangesAsync();
        }

        public async Task CompleteRequestAsync(int requestId)
        {
            var result = await context.Requests.FindAsync(requestId);

            if (result == null)
                throw new ArgumentException("The specified request does not exist.");

            result.Completed = true;

            await context.SaveChangesAsync();
        }

        public void AddClusterGroup(ClusterGroup newCluster)
        {
            context.ClusterGroups.Add(newCluster);
            context.SaveChanges();
        }

        public void RemoveClusterGroup(ClusterGroup cluster)
        {
            // attach the cluster to delete to the context, if needed. Otherwise the remove/delete won't work
            var cluster2Del = context.ClusterGroups.Local.FirstOrDefault(cc => cc.Id == cluster.Id);
            if (cluster2Del == null)
                context.ClusterGroups.Attach(cluster);

            context.ClusterGroups.Remove(cluster);
            context.SaveChanges();
        }

        public ClusterGroup UpdateClusterGroup(ClusterGroup updatedCluster)
        {
            var result = context.ClusterGroups.Find(updatedCluster.Id);

            if (result == null)
                throw new ClusterGroupNotFoundException();

            result.Name = updatedCluster.Name;
            result.Description = updatedCluster.Description;

            context.SaveChanges();

            return result;
        }

        public void AddDisasterCluster(DisasterCluster newDisasterCluster)
        {
            context.DisasterClusters.Add(newDisasterCluster);
            context.SaveChanges();
        }

        public void RemoveDisasterCluster(DisasterCluster newDisasterCluster)
        {
            context.DisasterClusters.Remove(newDisasterCluster);
            context.SaveChanges();
        }

        public Cluster UpdateCluster(Cluster updatedCluster)
        {
            var result = context.Clusters.Find(updatedCluster.Id);

            if (result == null)
                throw new ClusterNotFoundException();

            result.Name = updatedCluster.Name;

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
            // attach the coordinator to delete to the context, if needed. Otherwise the
            // remove/delete won't work
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