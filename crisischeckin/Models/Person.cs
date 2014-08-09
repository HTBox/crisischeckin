using System.Collections.Generic;
namespace Models
{
    public class Person
    {
        public int Id { get; set; }         // Used for identity in Person table.
        public int? UserId { get; set; }     // Used for membership provider and eventually an OAuth implementation
        public int? ClusterId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string FullName { get { return LastName + ", " + FirstName; } }

        public virtual Cluster Cluster { get; set; }
        public virtual ICollection<Commitment> Commitments { get; set; }
    }
}
