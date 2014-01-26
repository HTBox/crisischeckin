using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Models
{
    public class CrisisCheckin : DbContext
    {
        public DbSet<Commitment> Commitments { get; set; }
        public DbSet<Disaster> Disasters { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Cluster> Clusters { get; set; }
        public DbSet<ClusterCoordinator> ClusterCoordinators { get; set; }
        public DbSet<ClusterCoordinatorLogEntry> ClusterCoordinatorLogEntries { get; set; }
     
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
