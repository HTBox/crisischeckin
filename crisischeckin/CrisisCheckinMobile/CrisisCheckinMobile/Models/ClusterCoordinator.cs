using Breeze.Sharp;
namespace CrisisCheckinMobile.Models
{
    public class ClusterCoordinator :BaseEntity
    {
        public int Id { get { return GetValue<int>(); } set { SetValue(value); } }
        public int PersonId { get { return GetValue<int>(); } set { SetValue(value); } }
        public int DisasterId { get { return GetValue<int>(); } set { SetValue(value); } }
        public int ClusterId { get { return GetValue<int>(); } set { SetValue(value); } }

        public Person Person { get { return GetValue<Person>(); } set { SetValue(value); } }
        public Cluster Cluster { get { return GetValue<Cluster>(); } set { SetValue(value); } }
        public Disaster Disaster { get { return GetValue<Disaster>(); } set { SetValue(value); } }
    }
}