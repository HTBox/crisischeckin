namespace CrisisCheckinMobile.Models
{
    public class Person
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? ClusterId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get { return LastName + ", " + FirstName; } }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int OrganizationId { get; set; }
        public Cluster Cluster { get; set; }
        //  public NavigationSet<Commitment> Commitments { get; set; }
    }
}
