using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Person
    {
        public int Id { get; set; }         // Used for identity in Person table.

        public int? UserId { get; set; }     // Used for membership provider and eventually an OAuth implementation

        [StringLength(30), ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string FirstName { get; set; }

        [StringLength(30), ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string LastName { get; set; }

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string FullName { get { return LastName + ", " + FirstName; } }

        public virtual ICollection<Commitment> Commitments { get; set; }
    }
}
