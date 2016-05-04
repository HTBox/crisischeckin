using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Models
{
    public class CrisisCheckinMembership : DbContext
    {
        public CrisisCheckinMembership()
            : base("CrisisCheckin")
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }

    public class CrisisCheckin : DbContext
    {
        public DbSet<Commitment> Commitments { get; set; }

        public DbSet<Disaster> Disasters { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<ClusterGroup> ClusterGroups { get; set; }

        public DbSet<Cluster> Clusters { get; set; }

        public DbSet<VolunteerType> VolunteerTypes { get; set; }

        public DbSet<ClusterCoordinator> ClusterCoordinators { get; set; }

        public DbSet<ClusterCoordinatorLogEntry> ClusterCoordinatorLogEntries { get; set; }

        public DbSet<DisasterCluster> DisasterClusters { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<ResourceType> ResourceTypes { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<StringLengthAttributeConvention>();
            
        }
    }
}