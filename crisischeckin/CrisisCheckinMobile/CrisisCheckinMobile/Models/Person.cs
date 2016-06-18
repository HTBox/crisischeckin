namespace CrisisCheckinMobile.Models
{
    public class Person
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? ClusterId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get { return LastName + ", " + FirstName; } }
        public Cluster Cluster { get; set; }
        //  public NavigationSet<Commitment> Commitments { get; set; }
    }
}
