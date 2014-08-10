using Breeze.Sharp;
using System.Collections.Generic;
namespace CrisisCheckinMobile.Models
{
    public class Person : BaseEntity
    {
        public int Id { get { return GetValue<int>(); } set { SetValue(value); } }
        public int? UserId { get { return GetValue<int?>(); } set { SetValue(value); } }
        public int? ClusterId { get { return GetValue<int>(); } set { SetValue(value); } }
        public string FirstName { get { return GetValue<string>(); } set { SetValue(value); } }
        public string LastName { get { return GetValue<string>(); } set { SetValue(value); } }
        public string Email { get { return GetValue<string>(); } set { SetValue(value); } }
        public string PhoneNumber { get { return GetValue<string>(); } set { SetValue(value); } }

        public string FullName { get { return LastName + ", " + FirstName; } }

        public Cluster Cluster { get { return GetValue<Cluster>(); } set { SetValue(value); } }
        public NavigationSet<Commitment> Commitments { get { return GetValue<NavigationSet<Commitment>>(); } set { SetValue(value); } }
    }
}
