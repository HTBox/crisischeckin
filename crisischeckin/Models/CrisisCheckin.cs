using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CrisisCheckin : DbContext
    {
        public DbSet<Commitment> Commitments { get; set; }
        public DbSet<Disaster> Disasters { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
